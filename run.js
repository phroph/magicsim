var Q = require("Q");
var cartesianProduct = require("cartesian-product");
var fs = require("fs");
var exec = require('child_process').exec;

var region = process.argv[2];
var realm = process.argv[3];
var name = process.argv[4];

var config = { names: ["fighttime","fightstyle"], values: [[250, 400],["low_movement","high_movement","patchwerk"]]};

var cp = config.names.reduce(function(prev, cur) {
    var name= cur;
    var values =config.values[config.names.indexOf(cur)];
    var cp = cartesianProduct([[name], values]);
    return prev.concat([cp]);
},[]);


var configurations = cartesianProduct(cp);
fs

Q.nfcall(fs.readdir,"templates").then(function(templates) {
    return cartesianProduct([templates,configurations]);
}).then(function(sims){
    return Q.all(sims.map(function(sim){
        return Q.nfcall(fs.readFile, "templates/"+ sim[0], "utf-8").then(function(templateData){
            var fighttime;
            var fightstyle;
            sim[1].forEach(function(variable) {
                if(variable[0] == "fighttime") {
                    fighttime = variable[1];
                } else if(variable[0] == "fightstyle") {
                    fightstyle = variable[1]
                }
                templateData = templateData.replace("%" + variable[0] + "%",variable[1]);
            })
            templateData = templateData.replace("%region%",region);
            templateData = templateData.replace("%realm%",realm);
            templateData = templateData.replace("%name%",name);
            var filename = sim[0].replace("template", name+"_"+fighttime+"_"+fightstyle.replace("_", ""));
            templateData = templateData.replace("%filename%",filename);
            return {data: templateData,fileName: filename};
        }).then(function(data) {
            if (!fs.existsSync('./sims/')) { 
                fs.mkdirSync('./sims/');
            }
            return Q.nfcall(fs.writeFile, "./sims/"+data.fileName, data.data, "utf-8")
        });
    }))
}).then(function() {
    // start simc run
    if (!fs.existsSync('./results/')) { 
        fs.mkdirSync('./results/');
    }
    return Q.all(fs.readdirSync("sims").map(function(sim) {
        var deferred = Q.defer();
        exec("C:\\Users\\phrop\\Downloads\\simc-715-01-win64-a0a9385\\simc-715-01-win64\\simc.exe ../sims/" + sim, { cwd: "./results" }, function(err, out, stderr) {
            deferred.resolve();
        });
        return deferred;
    }));
}).then(function() {
    // process results.
}).done();