module.exports.run = function(window, cArgs) {

    var Q = require('q');
    var cartesianProduct = require('cartesian-product');
    var process_exec = require('child_process').exec;
    var execSync = require('child_process').execSync;
    var zip = require('node-7z')
    var xpath = require('xpath');
    var dom = require('xmldom').DOMParser;
    var http = require('http');
    var path = require('path');
    var locks = require('locks');
    var fs = require('fs');
    var os = require('os');
    var argv = require('yargs').argv;

    var boss_models = require("./models.js").models;
    var model;
    var modelName;
    var timeModel;
    var srcbuild;
    var advancedOperations;
    var simthreads = 1;

    var testMode = true;

    var region,realm,name,threads,weights,ptrl;
    if(cArgs!=null) {
        region = cArgs.region;
        realm = cArgs.realm;
        name = cArgs.character;
        threads = cArgs.theads;
        weights = cArgs.weights
        modelName = cArgs.model;
        ptr = cArgs.ptr;
        var m = boss_models.filter((m) => {
                return m.name == cArgs.model;
            })[0];
        model = m.model;
        timeModel = m.timeModel;
        // Advanced mode should only be available through the GUI tbh. Excepting M+ sims ofc, but those are a special case.
        // In that case AM should not be user exposed through CLI.
        advancedMode = cArgs.advancedMode;
        advancedOperations = cArgs.advancedOperations;
    } else {
        if(!argv.model) {
            modelName = 'nh';
            var m = boss_models.filter((m) => {
                return m.name == "nh";
            })[0]
            model = m.model;
            timeModel = m.timeModel;
        } else {
            modelName = argv.model;
            var m = boss_models.filter((m) => {
                return m.name == argv.model;
            })[0]
            model = m.model;
            timeModel = m.timeModel;
        }
        threads = argv.threads;
        weights = !argv.noweights;
        ptr = argv.ptr;
        srcbuild = argv.srcbuild;
        region = process.argv[2];
        realm = process.argv[3];
        name = process.argv[4];
    }
    if(testMode) {
        var m = boss_models.filter((m) => {
            return m.name == "universal";
        })[0]
        modelName = 'universal';
        model = m.model;
        timeModel = m.timeModel;
        threads = 1;
        simthreads = 36;
    }

    // Due to restrictions with SimC, we have to generate and modify the APL for dungeon sims.
    // ergo we must enforce advanced mode if it is not already enabled.
    if(modelName == 'mplus' || testMode) {
        advancedMode = true;
    }

    srcbuild = true;

    var config = { names: ["fighttime","fightstyle"], values: [[90, 250, 400],["low_movement","high_movement","patchwerk"]]};
    var addConfig = { names: ["fighttime","fightstyle","adds"], values: [[30,35,50,55,60],["low_movement","patchwerk"],['3','4','5']]}

    var cp = config.names.reduce(function(prev, cur) {
        var name= cur;
        var values =config.values[config.names.indexOf(cur)];
        var cp = cartesianProduct([[name], values]);
        return prev.concat([cp]);
    },[]);

    var addCp = addConfig.names.reduce(function(prev, cur) {
        var name= cur;
        var values =addConfig.values[addConfig.names.indexOf(cur)];
        var cp = cartesianProduct([[name], values]);
        return prev.concat([cp]);
    },[]);

    var deleteContents = function(path) {
        if(fs.existsSync(path) ) {
            fs.readdirSync(path).forEach(function(file,index){
                var curPath = path + "/" + file;
                if(fs.lstatSync(curPath).isDirectory()) { // recurse
                    deleteContents(curPath);
                    fs.rmdirSync(curPath);
                } else { // delete file
                    fs.unlinkSync(curPath);
                }
            });
        }
    }

    var pool = [];
    var poom_max = 36;
    for(var i = 0; i<36; i++) {
        pool.push(null);
    }
    var pool_size = 4;
    if(threads && threads >= 1 && threads <= 36) {
        pool_size = threads;
    }
    var workers = 0;

    var mutex = locks.createMutex();

    // Takes a command and options, executes it, and returns a promise that resolves/errors with the process, making sure the parent kills the child when it dies.
    var bindExec = function(command, options) {
        var defer = Q.defer();
        var proc = process_exec(command, options, (error, stdout, stderr) => {
            if(error) {
				console.log(error);
                defer.reject();
            } else {
                defer.resolve();
            }
        });

        if(window!=null) {
            window.on('close', () => {
                process_exec("taskkill /pid " + proc.pid + " /f /t")
            });
        }
        
     
        return defer.promise;
    }
    var baseRegex = '(?:actions(?:\\..*)?\\+?=)\\/?%name%,%attribute%=([\\w_]*).*\r\n(?:#?.*\r\n)*?(actions.*)';
    var variableMapping = {
        food: 'type',
        flask: 'type',
        potion: 'name',
        augmentation: 'type'
    }
    var getRegexFor = (command) => {
        if(command == 'race') {
            return new RegExp('race=(.*)');
        } else {
            return new RegExp(baseRegex.replace('%name%',command).replace('%attribute%',variableMapping[command]),'g')
        }
    }

    var exec = function(command, options, callback) {
        mutex.lock(function() {
            var return_promise;
            if (pool[workers%pool_size] == null) {
                console.log("Running: " + command);
                return_promise = bindExec(command, options).then(function() {
                    callback();
                }, (err) => {
                    console.log("Error: Failed to execute sim profile " + command);
                    throw err;
                });
                pool[workers%pool_size] = return_promise;
                workers++;
            } else {
                var chain = (pool[workers%pool_size]).then(function() {
                    console.log("Running: " + command);
                    return bindExec(command, options).then(function() {
                            callback();
                        }, (err) => {
                            console.log("Error: Failed to execute sim profile " + command);
                            throw err;
                        });
                });
                pool[workers%pool_size] = chain;
                console.log("Queued: " + command);
                workers++;
            }
            mutex.unlock();
        });
    }


    var configurations = cartesianProduct(cp);
    var addConfiguration = cartesianProduct(addCp);

    var simcPromise;
    if(srcbuild) {
        simcPromise = Q();
    } else {
        simcPromise = Q().then(function() {
            var deferred = Q.defer();
            var rawData = "";
            console.log("Downloading SimulationCraft version list.");
            http.get("http://downloads.simulationcraft.org/?C=M;O=D", (response) => {
                response.on('data', (chunk) => rawData += chunk);
                response.on('end', () => {
                    deferred.resolve(rawData);
                });
            });

            return deferred.promise;
        }).then((response) => {
                var doc = new dom().parseFromString(response, "text/html")
                var select = xpath.useNamespaces({ h: 'http://www.w3.org/1999/xhtml' });
                var arch = 'win64';
                if (os.arch() != 'x64') {
                    arch = 'win32';
                }
                var fname = select("//h:html/h:body/h:table/h:tr/h:td/h:a[contains(@href,'.7z') and contains(@href,'" + arch + "')][1]/@href", doc)[0].value
                var link = "http://downloads.simulationcraft.org/" + fname;
                console.log('Found: ' + link);
                if (fs.existsSync(fname)) { 
                    console.log("Skipping download. Already found existing file.");
                    return fname;
                }
                console.log("Downloading simc.7z for " + arch + " platform.");
                var file = fs.createWriteStream(fname);
                var deferred = Q.defer();
                http.get(link, (response) => {
                    response.pipe(file);
                    response.on('end', () => {
                        console.log("Done downloading simc.7z");
                        deferred.resolve(fname);
                    });
                })
                return deferred.promise;
        }).then((name) => {
            var deferred = Q.defer();
            var binFolder = path.join('.','bin');
            if (!fs.existsSync(binFolder)) { 
                fs.mkdirSync(binFolder);
            } else {
                deleteContents(binFolder);
            }

            console.log("Extracting simc.7z archive.");
            new zip().extractFull(name, 'bin', {})
            .then(() => {
                console.log("Done extracting simc.7z.");
                deferred.resolve();
            }, () => {
                fs.unlinkSync(name);
                deferred.reject('Failed to extract archive.');
            });
            return deferred.promise;
        })
    }
    simcPromise.then(() => { 
        var deferred = Q.defer();
        fs.readdir('templates', (err, files) => {
            if(err) {
                deferred.reject(err);
                return;
            }
            deferred.resolve(files);
        });
        return deferred.promise;
    }).then(function(templates) {
        var addTemplates = templates.filter(template => {
            return template.includes("adds");
        });
        var simTemplates = templates.filter(template => {
            return !template.includes("adds");
        });
        return (cartesianProduct([simTemplates, configurations])).concat(cartesianProduct([addTemplates,addConfiguration]));
    }).then(function(sims){
        var simsFolder = 'sims';
        if (!fs.existsSync(simsFolder)) { 
            fs.mkdirSync(simsFolder);
        }  else {
            deleteContents(simsFolder);
        }
        console.log("Generating simc profiles.");
        if(advancedMode) {
            // Delete old builder profiles
            console.log("Detecting advanced mode. Generating player profile using simc.");
            var builder = fs.readFileSync(path.join('build_profiles','builder.simc'),'utf-8');
            builder = builder.replace(/%region%/g,region);
            builder = builder.replace(/%realm%/g,realm);
            builder = builder.replace(/%name%/g,name);
            if(ptr) {
                builder = 'ptr=1\r\n' + builder; 
            }
            if(!fs.existsSync('profile_builder')) {
                fs.mkdirSync('profile_builder');
            }
            fs.writeFileSync(path.join('profile_builder', name + '.simc'), builder);
            try {
                var simcpath;
                if (srcbuild) {
                    simcpath = path.join("..", "simc", "engine", "simc");
                } else {
                    simcpath = path.join("..", "bin", fs.readdirSync("bin")[0], "simc.exe");
                }
                execSync(simcpath + " " + path.join('profile_builder', name + '.simc'));
            }
            catch(err) {
                console.log(err.toString());
                throw err;
            }
            // Generate profile.
            console.log("Performing specified modifications to APL.");
            // Advanced profile mutations
            var apl = fs.readFileSync(path.join('profile_builder', name + '.simc'),'utf-8');
            if(modelName == 'mythicplus') {
                // Do our special Mythic+ configuration steps.
                console.log('Applying special Mythic+ customizations.');
                if(advancedOperations == null) {
                    advancedOperations = [];
                }
        
                advancedOperations.push({name: 'potion', replacement: 'DELETE'});
            }

            if(advancedOperations != null) {
                advancedOperations.forEach((operation) => {
                    if(operation.name == 'setbonus' && operation.replacement != 'DEFAULT') {
                        // Extract the different tier configurations.
                        var sets = operation.replacement.split(';');
                        sets.forEach((set) => {
                            var regex = /(\d+):(\dpc=\d),(\dpc=\d)/;
                            var matches = set.match(regex);
                            var tier = matches[1];
                            var twopc = matches[2];
                            var fourpc = matches[3];
                            var baseStr = 'set_bonus=tier{0}_{1}';
                            if(tier == '20') {
                                ptr = true;
                            }
                            apl = apl + '\r\n' + baseStr.replace('{0}',tier).replace('{1}',twopc);
                            apl = apl + '\r\n' + baseStr.replace('{0}',tier).replace('{1}',fourpc);
                        })
                    } else {
                        var replacement = operation.replacement;
                        //var regex = escape(operation.regex);
                        var regex = getRegexFor(operation.name);
                        apl = apl.replace(regex, (match, g1, g2) => {
                            var newMatch = match;
                            if(replacement == 'DELETE') {
                                // Detect if deleting line will cause issues.
                                if(newMatch.match(/actions(?:\.[^\=\+]*)?=(?:[^\/\n]+)/)) {
                                    // Fix the next line we captured
                                    newMatch = newMatch.replace(g2,g2.replace('+=/','='))
                                }
                                // Now comment out the first line.
                                var firstLine = newMatch.split('\n')[0];
                                newMatch = newMatch.replace(firstLine, '#' + firstLine);
                            } else if(replacement != 'DEFAULT') {
                                newMatch = newMatch.replace(g1,replacement);
                            }
                            return newMatch
                        });
                    }
                });
            }
            fs.writeFileSync(path.join('profile_builder', name + '.simc'), apl);
        }
        return Q.all(sims.map(function(sim){
            return Q.nfcall(fs.readFile, "templates/"+ sim[0], "utf-8").then(function(templateData){
                var fighttime;
                var fightstyle;
                var adds = null;
                sim[1].forEach(function(variable) {
                    if(variable[0] == "fighttime") {
                        fighttime = variable[1];
                    } else if(variable[0] == "fightstyle") {
                        fightstyle = variable[1];
                    } else if(variable[0] == "adds") {
                        adds = variable[1];
                    }
                    
                    templateData = templateData.replace("%" + variable[0] + "%",variable[1]);
                })
                
                var replaceValue;
                var filename;
                if(adds != null) {
                    replaceValue = fighttime + '_' + fightstyle.replace("_", "")  + '_' + adds;
                    filename = sim[0].replace("template", replaceValue);
                    templateData = templateData.replace("%meta_totaltime%", 400 - 400%(fighttime+15));
                    templateData = templateData.replace("%nettime%", fighttime+15);
                } else {
                    replaceValue = fightstyle.replace("_", "");
                    filename = fighttime + '_' + sim[0].replace("template", replaceValue);
                }
                var modelName = sim[0].split('.')[0].replace("template", replaceValue); // IE 35_patchwerk_3_adds or patchwerk_ba_2t
                if(model[modelName] == null || model[modelName] == 0 || ((timeModel[fighttime] == null || timeModel[fighttime] == 0) && adds == null)) {
                    console.log("Preventing generation of unused model: " + modelName);
                    return null;
                }
                if(advancedMode) {
                    templateData = templateData.replace(/armory=%region%,%realm%,%name%/g,"input=../profile_builder/%name%.simc");
                }
                if(region == "sim_test") {
                    templateData = templateData.replace("#input=../test_profiles/h2priest_nh_%version%.simc","input=../test_profiles/h2priest_nh_%version%.simc");
                    templateData = templateData.replace(/%version%/g, "911");
                    templateData = templateData.replace(/armory=%region%,%realm%,%name%/g,"input=../test_profiles/characterbase.simc\r\ninput=../test_profiles/apl232017.simc");
                    region = "test";
                    realm = "test";
                    name = "sim_test";
                }
                if(false/*testMode*/) {
                    templateData = templateData.replace("#input=../test_profiles/h2priest_nh_%version%.simc","input=../test_profiles/h2priest_nh_%version%.simc");
                    templateData = templateData.replace(/%version%/g, "911");
                }
                if(ptr) {
                    templateData = "ptr=1\r\n" + templateData;
                }
                if(!weights) {
                    templateData = templateData.replace("calculate_scale_factors=1","#noweights");
                }
                templateData = templateData.replace(/%threads%/g,simthreads);
                templateData = templateData.replace(/%region%/g,region);
                templateData = templateData.replace(/%realm%/g,realm);
                templateData = templateData.replace(/%name%/g,name);
                filename = name + '_' + filename;
                var prefix = fs.readFileSync(path.join('build_profiles','prefix.simc'), 'utf-8');
                var postfix = fs.readFileSync(path.join('build_profiles','postfix.simc'), 'utf-8');
                templateData = templateData.replace("#aplhook", "input=../build_profiles/aplfix.simc");
                templateData = prefix + '\r\n' + templateData + '\r\n' + postfix + '\r\n';
                if(testMode) {
                    templateData = templateData + 'reforge_plot_stat=crit,mastery,haste\r\nreforge_plot_amount=8000\r\nreforge_plot_step=500\r\nreforge_plot_iterations=2000\r\nreforge_plot_output_file=%filename%.csv'
                }
                templateData = templateData.replace(/%filename%/g,filename);

                console.log("Generating simc profile: " + filename);
                return {data: templateData,fileName: filename};
            }).then(function(data) {
                if(data == null) { 
                    return;
                }
                return Q.nfcall(fs.writeFile, path.join(simsFolder,data.fileName), data.data, "utf-8")
            });
        }))
    }).then(function() {
        var resultsFolder = path.join('.','results');
        if (!fs.existsSync(resultsFolder)) { 
            fs.mkdirSync(resultsFolder);
        } else {
            deleteContents(resultsFolder);
        }
        console.log("Starting SimulationCraft run");
        var promises = fs.readdirSync("sims").map(function(sim) {
            var simcpath;
            if (srcbuild) {
                simcpath = path.join("..", "..", "simc", "engine", "simc");
            } else {
                simcpath = path.join("..", "bin", fs.readdirSync("bin")[0], "simc.exe");
            }
            var cmd = simcpath + " " + path.join("..","sims", sim);
            
            var deferred = Q.defer();
			// dangerously large buffer, but for reforge, gotta live large.
            exec(cmd, { cwd: resultsFolder, maxBuffer: 1024*1024*1024 }, function() {
                console.log("Done sim: " + sim);
                deferred.resolve();
            });
            return deferred.promise;
        });
        return Q.allSettled(promises).then(function() {
            var deferral = Q.defer();
            console.log('Done Simming.');
            var analyze = path.join('.','analyze.js') + " " + region;
            if(!weights) {
                analyze += " --noweights"; 
            }
            analyze += " --model " + modelName;
            process_exec("node " + analyze, function(err, out, stderr) {
                console.log(out);
                console.log(stderr);
                console.log('Done Analysis.');
                if(testMode) {
                    boss_models.forEach(model => {
                        execSync("node bulkalyze.js --model " + model.name);
                    })
                }
                deferral.resolve();
            });
            return deferral.promise;
        });
    }).fail((err) => {
        console.log("Error: Failed to process with exeception: " + err);
    }).done();
}