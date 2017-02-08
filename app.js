var express = require('express')
var path = require('path');
var app = express()

app.get('/', function (req, res) {
  res.sendFile(path.join(__dirname, 'web', 'index.html'), {}, function(err) {
    if (err) {
        next(err);
    }
  });
})

app.listen(3000, function () {
  console.log('Example app listening on port 3000!')
})