var AWS = require('aws-sdk');
var fs = require('fs');

AWS.config.update({region:'us-east-1'});
var s3 = new AWS.S3();
s3.upload({
    Bucket: 'atmasim',
    Key: 'bin/atmasimDriver.jar',
    Body: fs.readFileSync('build/libs/atmasimDriver.jar'),
    ContentType: "text/plain",
    ACL: 'public-read'
    },function (err, data) {
        if(err) {
            console.log(err);
            return;
        }
    }
);