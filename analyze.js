var xpath = require("xpath");
var dom = require('xmldom').DOMParser;
var fs = require("fs");

var fight_mapping = {
    patchwerk_ba_2t: 0.025,
    patchwerk_ba_st: 0.07,
    patchwerk_sa_2t: 0.02,
    patchwerk_sa_st: 0.07,
    patchwerk_na_2t: 0,
    patchwerk_na_st: 0.07,
    lowmovement_ba_2t: 0.155,
    lowmovement_ba_st: 0.195,
    lowmovement_sa_2t: 0.05,
    lowmovement_sa_st: 0.12,
    lowmovement_na_2t: 0.03,
    lowmovement_na_st: 0.065,
    highmovement_ba_2t: 0,
    highmovement_ba_st: 0.08,
    highmovement_sa_2t: 0.015,
    highmovement_sa_st: 0.035,
    highmovement_na_2t: 0,
    highmovement_na_st: 0
}

var time_mapping = {
    250: .4,
    400: .6
}

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

var int = 0;
var mastery = 0;
var haste = 0;
var crit = 0;
var sp = 0;
var vers = 0;
fs.readdirSync("./results").forEach(function(result){
    var xml = fs.readFileSync(path.join('.','results') + result, "utf-8");
    var doc = new dom().parseFromString(xml);
    var res = result.match("^(.*)_([0-9]*)_(.*).simc.xml");
    var name = res[1];
    name = name.charAt(0).toUpperCase() + name.slice(1);
    var time = res[2];
    var fight = res[3];
    var weight = fight_mapping[fight]*time_mapping[time];
    int += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Int']/@value", doc).value*weight;
    mastery += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Mastery']/@value", doc).value*weight;
    haste += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Haste']/@value", doc).value*weight;
    crit +=xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Crit']/@value", doc).value*weight;
    sp += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='SP']/@value", doc).value*weight;
    vers += xpath.select1("//simulationcraft/players/player[@name='"+name+"']/scale_factors/metric[@name='" + name + " Damage Per Second']/weights/stat[@name='Vers']/@value", doc).value*weight;
    sum += weight;
});

console.log("Total calibration: " + sum);
console.log({int:int/int,mastery:mastery/int,haste:haste/int,crit:crit/int,sp:sp/int,vers:vers/int});

console.log("Note: If your calibration values vary greatly from 1, please verify weights/that all sims were run.");
