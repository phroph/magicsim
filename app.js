var express = require('express');
var path = require('path');
var app = express();
var bodyParser = require('body-parser');
var boss_models = require("./models.js").models;

var oldLog = console.log;
app.use(express.static(path.join(__dirname,'web_res')));
app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());

app.get('/', function (req, res) {
    res.sendFile(path.join(__dirname, 'web', 'index.html'), {}, function(err) {
        if (err) {
            next(err);
        }
    });
})

var running = false;
var analysisMode = false;
var damageString = '';
var pawnString = '';
var errString = '';
var state = '';
var simsString = '';
var doneSims = 0;
var totalSims = 0;

app.get('/sim/models', function (req, res) {
    res.send(JSON.stringify(boss_models.map((model) => {
        return {name:model.name, dispName:model.dispName};
    })));
})

app.get('/sim/results', function (req, res) {
    if(running || analysisMode) {
        res.send(JSON.stringify({ done: doneSims, total: totalSims, state: state }));
    } else if(errString != '') {
        res.send(JSON.stringify({ error: errString }));
        errString = '';
    } else if(damageString != '') {
        res.send(JSON.stringify({ damage: damageString, pawn: pawnString, sims: simsString}));
        pawnString = '';
        damageString = '';
    } else {
        res.send(false);
    }
})

var processLine = function(line) {
    if(line.includes('Done Simming.')) {
        analysisMode = true;
        state = 'Analyzing';
    }
    if(analysisMode && line.includes('Done Analysis.')) {
        if(damageString == '') {
            errString = 'No analysis could be found. Please check logs and try again.';
        }
        analysisMode = false;
        running = false;
    } else if(line.includes('Done Analysis.')) {
        errString = 'No analysis could be found. Please check logs and try again.';
    }
    if(analysisMode && line.includes('Damage (DPS):')) {
        damageString = line;
    }
    if(analysisMode && line.includes('Pawn: v1:')) {
        pawnString = line;
    }
    if(line.includes('Done sim:')) {
        doneSims++;
    }
    if(line.includes('Generating simc profile:')) {
        if(simsString == '') {
            simsString = line.match("profile: (.+).simc")[1];
        } else {
            simsString += '\n' + line.match("profile: (.+).simc")[1];
        }
        totalSims++;
    }
    if(line.includes('Downloading simc.7z')) {
        state = 'Downloading';
    }
    if(line.includes('Extracting simc.7z')) {
        state = 'Extracting';
    }
    if(line.includes('Starting SimulationCraft run')) {
        state = 'Simulating';
    }
}

app.post('/sim/request', function (req, res) {
    var char, region, realm;
    char = req.body.character;
    region = req.body.region;
    realm = req.body.realm;
    if(!running) {
        running = true;
        doneSims = 0;
        totalSims = 0;
        analysisMode = false;
        damageString = '';
        pawnString = '';
        errString = '';
        simsString = '';
        state = 'Initializing';
        var execString = 'node run.js ' + region + ' ' + realm + ' ' + char;
        if(req.body.threads) {
            execString += " --threads " + req.body.threads;
        }
        if(req.body.noweights == "true") {
            execString += " --noweights";
        }
        if(req.body.ptr == "true") {
            execString += " --ptr";
        }
        if(req.body.model) {
            execString += " --model " + req.body.model;
        }

        var args = {
            region: region,
            realm: realm,
            character: char,
            theads: req.body.threads,
            weights: req.body.noweights == "true" ? false: true,
            ptr: req.body.ptr == "true",
            model: req.body.model,
            advancedMode: req.body.advancedMode,
            advancedOperations: []
        }
        if(req.body.advancedOperations) {
            args.advancedOperations = JSON.parse(req.body.advancedOptions);
        }
        console.log = function (data) {
            oldLog(data);
            try {
                if(data.includes('Error:')) {
                    errString = data;
                    running = false;
                }
                var lines = data.split('\n');
                lines.forEach((line) => {
                    processLine(line);
                });
            } catch(e) {

            }
        }

        require('./runModule.js').run(exports.window, args);

        exports.window.on('close', () => {
            setTimeout(() => { exports.window.close(true);}, 100);
        })

        // Bind the output so we can read it.
        process.stdout.on('data', (data) => {
            console.log(data);
        });

        process.stderr.on('data', (data) => {
            console.log(data);
        });
        res.send(true);
    } else {
        res.send(false);
    }
})

app.listen(4000, function () {
  console.log('magicsim local backend live and operational.')
})

module.exports.version = "v1.2";