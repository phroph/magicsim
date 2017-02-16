var express = require('express');
var path = require('path');
var app = express();
var bodyParser = require('body-parser');
var exec = require('child_process').exec;

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
var doneSims = 0;
var totalSims = 0;

app.get('/sim/results', function (req, res) {
    if(running || analysisMode) {
        res.send(JSON.stringify({ done: doneSims, total: totalSims, state: state }));
    } else if(errString != '') {
        res.send(JSON.stringify({ error: errString }));
        errString = '';
    } else if(pawnString != '' && damageString != '') {
        res.send(JSON.stringify({ damage: damageString, pawn: pawnString}));
        pawnString = '';
        damageString = '';
    } else {
        res.send(false);
    }
})

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
        state = 'Initializing';
        var proc = exec('node run.js ' + region + ' ' + realm + ' ' + char);
        
        // Bind the output so we can read it.
        proc.stdout.on('data', (data) => {
            var lines = data.split('\n');
            lines.forEach((line) => {
                if(line.includes('Done Simming.')) {
                    analysisMode = true;
                    state = 'Analyzing';
                }
                if(analysisMode && line.includes('Done Analysis.')) {
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
            });
            console.log(data);
            if(data.includes('error')) {
                errString = data;
                running = false;
            }
        });

        proc.stderr.on('data', (data) => {
            var lines = data.split('\n');
            for(var line in lines) {
                if(line.includes('Done Simming.')) {
                    analysisMode = true;
                }
                if(analysisMode && line.includes('Done Analysis.')) {
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
                    totalSims++;
                }
            }
            console.log(data);
            if(data.includes('error')) {
                errString = data;
                running = false;
            }
        });
        res.send(true);
    } else {
        res.send(false);
    }
})

app.listen(4000, function () {
  console.log('magicsim local backend live and operational.')
})