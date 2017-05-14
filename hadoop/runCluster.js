var AWS = require('aws-sdk');
var Guid = require('guid');

AWS.config.update({region:'us-east-1'});
var s3 = new AWS.S3();
var emr = new AWS.EMR();

var bucket = 'atmasim';

// Create request ID from GUID.
// Create data request in the proper format.
// Using the s3 bucket with name atmasim.
// Save it as key input-<GUID>.dat.
// Create key results-<GUID>.
// Construct EMR request with proper input name / results folder.

var guid = Guid.create().value;
var jobFlowData = '';

var combineTalents = function(choices) {
    var possibleChoices = [[]];
    choices.forEach((options) => {
        newPossibilities = [];
        possibleChoices.forEach((possibility) => {
            options.forEach((option) => {
                var p = possibility.slice();
                p.push(option)
                newPossibilities.push(p);
            })
        })
        possibleChoices = newPossibilities;
    });
    return possibleChoices;
}

var combineSims = function(model) {
    // Simply combine time and model strings. Since NH, no adds to worry about.
    var  sims = [];
    for (var sim in Object.keys(model.model)) {
        for (var time in Object.keys(model.timeModel)) {
            sims = sims.concat(Object.keys(model.timeModel)[time] + "_" + Object.keys(model.model)[sim]);
        }
    }
    return sims;
}

var combineReforge = function(params) {
    var totalBudget = params.budget;
    var floor = params.floor;
    var ceiling = params.ceiling;
    var stepSize = params.step;
    var createState = function(c,m,h) {
        return {
            crit: c,
            mastery: m,
            haste: h,
            remainingBudget: totalBudget - (c+m+h),
            children: function() {
                if(this.remainingBudget < stepSize) {
                    return [];
                }
                var children = [];
                if(this.crit+stepSize<=ceiling) {
                    children.push(createState(this.crit+stepSize,this.mastery,this.haste));
                }
                if(this.mastery+stepSize<=ceiling) {
                    children.push(createState(this.crit,this.mastery+stepSize,this.haste));
                }
                if(this.haste+stepSize<=ceiling) {
                    children.push(createState(this.crit,this.mastery,this.haste+stepSize));
                }
                return children;
            },
            stringForm: function() {
                return "c:" + this.crit + ",m:" + this.mastery + ",h:" + this.haste;
            }
        }
    }
    var discard = {};
    var leaves = []; // Leaves are fully itemized and are the only nodes we want to look at.
    var threshold = [createState(floor,floor,floor)];
    while(threshold.length>0) {
        var cursor = threshold.pop();
        if(discard[cursor.stringForm()]) {
            continue;
        }
        var children = cursor.children();
        if(children.length==0) {
            leaves.push(cursor.stringForm());
        } else {
            threshold = threshold.concat(children);
        }
        discard[cursor.stringForm()] = true;
    }
    return leaves;
}

var simModel = require('../models.js').models[1]; // Nighthold
var simCombinations = combineSims(simModel); // 28 Combinations
console.log('Found ' + simCombinations.length + ' sim combinations.');
var talentChoices = [[0,1,2],[0],[0],[0,1],[0,1,2],[0],[0,2]];
var talentCombinations = combineTalents(talentChoices); // 36 Combinations
console.log('Found ' + talentCombinations.length + ' talent combinations.');
var reforgeParameters = {budget: 24000, step: 500, floor: 3000, ceiling: 17000}; 
// At each step, 500 can go 1 way, with a maximum of 17000 in any single way. Aka n^3 expansion, pruning duplicates and constraint failures.
// Floor is budget force allocated each way at least. So effective budget = budget - way*floor.
// 11000 available budget, 22 steps. 22^3 = O(10,648) reforge points, including duplicates.
var reforgeCombinations = combineReforge(reforgeParameters); // << 267 points
console.log('Found ' + reforgeCombinations.length + ' reforge combinations.');

// Now shit gets real. We take the cartesian product of all 3 of these basically. And line-by-line add records into jobFlowData.

var numJobs = 0;
simCombinations.forEach((sim) => {
    talentCombinations.forEach((talent) => {
        reforgeCombinations.forEach((reforge) => {
            jobFlowData += sim + ';' + talent + ';' + reforge + '\n';
            numJobs++;
        })
    })
})
console.log('Found ' + numJobs + ' jobs.');

s3.upload({
    Bucket: bucket,
    Key: 'input-' + guid + '.txt',
    Body: jobFlowData,
    ACL: 'public-read'
    },function (err, data) {
        if(err) {
            console.log(err);
            return;
        }
        console.log('Successfully uploaded input data: ' + 'input-' + guid + '.txt');
        s3.putObject({ Bucket: bucket, Key: 'results-' + guid + '/', Body: '' }, function(err, data) {
            if (err) {
                console.log(err)
            } else {
                console.log("Successfully created folder: " +  'results-' + guid + '/' );
                emr.runJobFlow({
                    Name: "Atmasim Job Flow",
                    Instances: {
                        KeepJobFlowAliveWhenNoSteps: false,
                        TerminationProtected: false,
                        InstanceGroups: [{
                            Name: "Master Instance Group",
                            InstanceRole: "MASTER",
                            InstanceCount: 1,
                            InstanceType: "c4.8xlarge",
                            Market: "ON_DEMAND"
                        }, {
                            Name: "Core Instance Group",
                            InstanceRole: "CORE",
                            InstanceCount: 1,
                            InstanceType: "c4.8xlarge",
                            Market: "ON_DEMAND"
                        }]
                    },
                    JobFlowRole: "EMR_EC2_DefaultRole",
                    ServiceRole: "EMR_DefaultRole",
                    Steps: [{
                        Name: "Atmasim Driver",
                        ActionOnFailure: "CANCEL_AND_WAIT",
                        HadoopJarStep: {
                            Jar: "s3://atmasim/atmasimDriver.jar",
                            Args: [
                                "s3://atmasim/input-" + guid + ".txt",
                                "s3://atmasim/results-" + guid + "/"
                            ]
                        }
                    }],
                    BootstrapActions: [ 
                    { 
                        Name: "Install SimC",
                        ScriptBootstrapAction: { 
                            Path: "s3://atmasim/installSimC.sh"
                        }
                    }
                    ],
                    LogUri: "s3://atmasim/logs-" + guid + "/",
                    VisibleToAllUsers: false,
                    ReleaseLabel: "emr-5.5.0"
                }, (err, data) => {
                    if(err) {
                        console.log(err);
                        return;
                    }
                    console.log(data);
                })
            }
        });
    });
