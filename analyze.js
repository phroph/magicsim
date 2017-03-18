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
    modelname = "nighthold";
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

sum = 0;

var damage = 0;
var dps = 0;
var main = 0;
var mainLabel = '';
var mastery = 0;
var haste = 0;
var crit = 0;
var sp = 0;
var simname;
var vers = 0;

var spec;
var type;

var results = fs.readdirSync(path.join(".","/results"));

results.forEach(function(result){
    var xml = fs.readFileSync(path.join('.','results', result), "utf-8");
    var doc = new dom().parseFromString(xml);
    var res = result.match("^(.*)_([0-9]*)_(.*).simc.xml");
    var addRes = result.match("^([^_]*)_(.*).simc.xml");

    var time = res[2];
    var fight = res[3];
    var weight;
    
    if(result.includes('adds')) {
        weight = fight_mapping[addRes[2]];
    } else {
        weight = fight_mapping[fight]*time_mapping[time];
    }
	damage += xpath.select1("//simulationcraft/summary/dmg/@total", doc).value*weight;
	dps += xpath.select1("//simulationcraft/summary/dmg/@dps", doc).value*weight;
    var name = xpath.select1("//simulationcraft/players/player[1]/@name", doc).value
    
	spec = xpath.select("//simulationcraft/players/player[@name='"+name+"']/specialization", doc)[0].firstChild.data;
	type = xpath.select1("//simulationcraft/players/player[@name='"+name+"']/class/@type", doc).value;
	if(name != "sim_test") {
		name = xpath.select1("//simulationcraft/summary/player_by_dps/player/@name", doc).value;
        simname = name;
	}
    if(!argv.noweights) {
        try {
            main += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Int']/@value", doc).value*weight;
			mainLabel = "Intellect";
		}
		catch(err) {
            try {
                main += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Str']/@value", doc).value*weight;
                mainLabel = "Strength";
            } catch(err) {
                main += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Agi']/@value", doc).value*weight;
                mainLabel = "Agility";
            }
		}
        try {
            mastery += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Mastery']/@value", doc).value*weight;
        }
        catch(e) {}
        haste += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Haste']/@value", doc).value*weight;
        crit += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Crit']/@value", doc).value*weight;
        vers += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Vers']/@value", doc).value*weight;
    }
    sum += weight;
});

if(process.argv[2] == "sim_test") {
    simname = "simtest";
}
console.log("Total calibration: " + sum);
console.log("Damage (DPS): " + damage + " (" + dps + ")");
if(!argv.noweights) {
    var smain = main/main;
    var smastery = mastery/main;
    var shaste = haste/main;
    var scrit = crit/main;
    var svers = vers/main;
    spec = spec.charAt(0).toUpperCase() + spec.slice(1);
    type = type.charAt(0).toUpperCase() + type.slice(1);
    console.log("( Pawn: v1: \"" + simname + "_" + modelname + "_selfsim\": Class=" + type + ", Spec=" + spec + ", " + mainLabel+"=" + smain + ", Versatility="+ svers.toFixed(4) + ", HasteRating=" + shaste.toFixed(4) + ", MasteryRating=" + smastery.toFixed(4) + ", CritRating=" + scrit.toFixed(4) + " )");
    if(process.argv[2] == "sim_test") {
        console.log("Expected Values: ( Pawn: v1: \"SL 4-piece\": Intellect=1, MasteryRating=1.36, HasteRating=1.2, CritRating=1.02, Versatility=0.9 )")
        console.log("Differences: int=" + (1-smain) + ", mastery=" + (1.36-smastery) + ", haste=" + (1.2-shaste) + ", crit=" + (1.02-scrit) + ", vers=" + (0.9-svers));
    }
}

console.log("Note: If your calibration values vary greatly from 1, please verify weights/that all sims were run.");
