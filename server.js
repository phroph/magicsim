var findpath = require('nw').findpath;
var nwpath = findpath();

require('./app.js');

const spawn = require('child_process').spawn;
const proc = spawn(nwpath, ['.']);

function exitHandler(options, err) {
    proc.kill();
    process.exit();
}

// Cross binding the processes so they both die and live together.
process.on('exit', exitHandler);
process.on('SIGINT', exitHandler);
process.on('uncaughtException', exitHandler);

proc.on('exit', exitHandler);
proc.on('SIGINT', exitHandler);
proc.on('uncaughtException', exitHandler);

process.stdin.resume();//so the program will not close instantly