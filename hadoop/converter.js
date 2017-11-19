var boss_models = require('../models.js').models;
var fs = require('fs');

fs.writeFileSync("./src/main/resources/models.json", JSON.stringify(boss_models));