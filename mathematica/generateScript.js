var fs = require("fs");

var baseScript = fs.readFileSync("baseScript.math","utf-8");

//"https://s3.amazonaws.com/atmasim/out/results-%guid%/%talent%/%model%-r-00000"
var resultGuid = process.argv[2];
var processString = 'processUrl["https://s3.amazonaws.com/atmasim/out/results-%guid%/%talent%/%model%-r-00000","%talent%","%model%"]'.replace(/%guid%/g,resultGuid);
var talentCombinations = [
    "1111111",
    "1111131",
    "1111211",
    "1111231",
    "2111111",
    "2111131",
    "2111211",
    "2111231"
]
var modelNames = [
    "tos"
]
var fullScript = baseScript;
for(var talent in talentCombinations) {
    talent = talentCombinations[talent]
    for(var model in modelNames) {
        model = modelNames[model]
        fullScript = fullScript + "\n" + processString.replace(/%model%/g,model).replace(/%talent%/g,talent);   
    }
}
fs.writeFileSync("process-" + resultGuid + ".math", fullScript);