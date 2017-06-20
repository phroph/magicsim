var xpath = require("xpath");
var dom = require('xmldom').DOMParser;
var fs = require("fs");
var path = require('path');
var argv = require('yargs').argv;
var boss_models = require("./models.js").models;
var fight_mapping;
var time_mapping

var modelname;
if(!argv.model) {
    modelname = "tos";
} else {
    modelname = argv.model;
}
fight_mapping = boss_models.filter((m) => {
    return m.name == modelname;
})[0].model;

time_mapping = boss_models.filter((m) => {
    return m.name == modelname;
})[0].timeModel;

var sum = 0;
Object.keys(time_mapping).forEach(function(time) {
    sum += time_mapping[time];
})
console.log("Time calibration: " + sum);
sum = 0;
Object.keys(fight_mapping).forEach(function(fight) {
    sum += fight_mapping[fight];
})
console.log("Fight calibration: " + sum);

var results = fs.readdirSync(path.join(".","/results"));

// Map name to dataPoints
var reforgeMapping = {};
var dataPoints;

results.forEach(function(result){
	if(!result.match(".csv")) {
		return;
	}
    var csv = fs.readFileSync(path.join('.','results', result), "utf-8");
    var res = result.match("^(.*)_([0-9]*)_(.*).simc.csv");
    var addRes = result.match("^([^_]*)_(.*).simc.csv");
	var lines = csv.split("\r\n").slice(2);

	console.log(result);
    var time = res[2];
    var fight = res[3];
    var weight;
    
    if(result.includes('adds')) {
        weight = fight_mapping[addRes[2]];
    } else {
        weight = fight_mapping[fight]*time_mapping[time];
    }
	if(!weight) {
		return;
	}

	console.log("Found csv valid for model: " + result);
	
	for(var i = 0; i < lines.length; i++) {
		var name = line.match("(/w+) Reforge Plot Results:")[1];
		if(name) {
			console.log("Found Reforge information for: " + name);
			if(!reforgeMapping[name]) {
				reforgeMapping[name] = {};
			}
			dataPoints = reforgeMapping[name];
		}
		var data = lines[i].split(",");
		var dataPoint = data[0] + ", " + data[1] + ", " + data[2];
		if(!dataPoints[dataPoint]) {
			dataPoints[dataPoint] = 0;
		}
		dataPoints[dataPoint] += Number(data[3])*weight;
	}
});

var fileContent = "Altrius Reforge Plot Results:\r\ncrit_rating, haste_rating, mastery_rarting, DPS\r\n";

for (var name in Object.keys(reforgeMapping)) {
	var dataPointList = reforgeMapping[name];
	for (var key in dataPointList) {
		if (dataPoints.hasOwnProperty(key)) {
			fileContent+= key + ", " + dataPoints[key] + ",\r\n";
		}
	}
	var fname = name + '_' + modelname + '.csv';
	fs.writeFileSync(path.join('.','results',fname), fileContent);
}
