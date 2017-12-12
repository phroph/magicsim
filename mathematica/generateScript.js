var fs = require("fs");

var baseScript = fs.readFileSync("baseScript.math","utf-8");

//"https://s3.amazonaws.com/atmasim/out/results-%guid%/%talent%/%model%-r-00000"
var resultGuid = process.argv[2];
var processString = 'processUrl["https://s3.amazonaws.com/atmasim/out/results-%guid%/%talent%/%model%/%leg1%and%leg2%%relics%and%crucible%-r-00000","%talent%","%model%"]'.replace(/%guid%/g,resultGuid);
var talentCombinations = [
    "1111231",
    "2111231",
]
var soulTalentCombinations = [
    "2111231",
    "2111232"
]
var legendaries = [
    "sephuz"
]
var modelNames = [
    "ant",
    "goroth"
]
var fullScript = baseScript;
for(var talent in talentCombinations) {
    talent = talentCombinations[talent]
    for(var model in modelNames) {
        model = modelNames[model]
        for(var legendary in legendaries) {
            legendary = legendaries[legendary]
            for(var i = 0; i<4; i++) {
                var relics = "1573-".repeat(i) + "775-".repeat(5-i) + "775";
                fullScript = fullScript + "\n" + processString.replace(/%model%/g,model).replace(/%talent%/g,talent).replace(/%leg1%/g,"mangaza").replace(/%leg2%/g,legendary).replace(/%relics%/g,relics).replace(/%crucible%/g,"1739-3");   
            }
        }
    }
}
for(var talent in soulTalentCombinations) {
    talent = soulTalentCombinations[talent]
    for(var model in modelNames) {
        model = modelNames[model]
        for(var i = 0; i<4; i++) {
            var relics = "1573-".repeat(i) + "775-".repeat(5-i) + "775";
            fullScript = fullScript + "\n" + processString.replace(/%model%/g,model).replace(/%talent%/g,talent).replace(/%leg1%/g,"mangaza").replace(/%leg2%/g, "soul").replace(/%relics%/g,relics).replace(/%crucible%/g,"1739-3");   
        }
    }
}
for(var talent in talentCombinations) {
    talent = talentCombinations[talent]
    for(var model in modelNames) {
        model = modelNames[model]
        for(var i = 0; i<4; i++) {
            var relics = "1573-".repeat(i) + "775-".repeat(5-i) + "775";
            fullScript = fullScript + "\n" + processString.replace(/%model%/g,model).replace(/%talent%/g,talent).replace(/%leg1%/g,"none").replace(/%leg2%/g, "none").replace(/%relics%/g,relics).replace(/%crucible%/g,"1739-3");   
        }
    }
}
fs.writeFileSync("process-" + resultGuid + ".math", fullScript);