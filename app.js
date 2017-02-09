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
var errString = 

app.get('/sim/results', function (req, res) {
    if(running || analysisMode) {
        res.send(false);
    } else if(errString != '') {
        res.send(errString);
        errString = '';
    } else if(pawnString != '' && damageSTring != '') {
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
        var proc = exec('node run.js ' + region + ' ' + realm + ' ' + char);
        
        // Bind the output so we can read it.
        proc.stdout.on('data', (data) => {
            if(data.includes('Done Simming.')) {
                analysisMode = true;
            }
            if(analysisMode && data.includes('Done Analysis.')) {
                analysisMode = false;
                running = false;
            } else if(data.includes('Done Analysis.')) {
                errString = 'No analysis could be found. Please check logs and try again.';
            }
            if(analysisMode && data.includes('Damage (DPS):')) {
                damageString = data;
            }
            if(analysisMode && data.includes('Pawn: v1:')) {
                pawnString = data;
            }
            console.log(data);
        });

        proc.stderr.on('data', (data) => {
            console.log(data);
        });
        res.send(true);
    } else {
        res.send(false);
    }
})

app.listen(3000, function () {
  console.log('magicsim local backend live and operational.')
})