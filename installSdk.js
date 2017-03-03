var http = require('http');
var unzip = require('unzip');
var fs = require('fs');
var arch = process.argv[2];
var path = require('path');
var mkdirp = require('mkdirp');
if(arch == 'win64') {
    var link = 'http://dl.nwjs.io/v0.20.3/nwjs-v0.20.3-win-x64.zip';
} else {
    var link = 'http://dl.nwjs.io/v0.20.3/nwjs-v0.20.3-win-ia32.zip';
}

http.get(link, (response) => {
    var fname = link.split('/v0.20.3/')[1];
    var file = fs.createWriteStream(fname);
    response.pipe(file).on('finish', function() {
        fs.createReadStream(fname).pipe(unzip.Parse()).on('entry', (entry) => {
            var archString = (arch=='win64' ? 'x64' : 'ia32');
            var relPath = entry.path.split('v0.20.3-win-'+ archString +'/')[1];
            var fpath = path.join('../build/magicsim-master', arch, relPath);
            mkdirp(path.dirname(fpath), function() {
                entry.pipe(fs.createWriteStream(fpath));
            })
        }).on('finish', () => {
            fs.unlinkSync(fname);
        })
    });
})