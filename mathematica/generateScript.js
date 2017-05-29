var fs = require("fs");

var baseScript = fs.readFileSync("baseScript.math","utf-8");

//"https://s3.amazonaws.com/atmasim/out/results-%guid%/%talent%/%model%-r-00000"
var resultGuid = "ee702cc5-5c3f-e963-73ea-0ac184d28131";
var talentCombinations = [
    "0000000",
    "0000020",
    "0000100",
    "0000120",
    "1000000",
    "1000020",
    "1000100",
    "1000120"
]
var modelNames = [
    "nh",
    "krosus"
]
baseScript = baseScript.replace("%guid%",resultGuid)
var fullScript = "";
for(var talent in talentCombinations) {
    talent = talentCombinations[talent]
    for(var model in modelNames) {
        model = modelNames[model]
        fullScript = fullScript + baseScript.replace(/%talent%/g,talent).replace(/%model%/g,model) + "\n";   
    }
}
fs.writeFileSync("process-" + resultGuid + ".math", fullScript);