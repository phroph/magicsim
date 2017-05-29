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
    var intellect = params.intellect;
    var ceiling = params.ceiling;
    var stepSize = params.step;
    var createState = function(c,m,h,i) {
        return {
            crit: c,
            mastery: m,
            haste: h,
            intellect: i,
            remainingBudget: totalBudget - (c+m+h),
            children: function() {
                if(this.remainingBudget < stepSize) {
                    return [];
                }
                var children = [];
                if(this.crit+stepSize<=ceiling) {
                    children.push(createState(this.crit+stepSize,this.mastery,this.haste,this.intellect));
                }
                if(this.mastery+stepSize<=ceiling) {
                    children.push(createState(this.crit,this.mastery+stepSize,this.haste,this.intellect));
                }
                if(this.haste+stepSize<=ceiling) {
                    children.push(createState(this.crit,this.mastery,this.haste+stepSize,this.intellect));
                }
                return children;
            },
            stringForm: function() {
                return "c:" + this.crit + ",m:" + this.mastery + ",h:" + this.haste + ",i:" + this.intellect;
            }
        }
    }
    var discard = {};
    var leaves = []; // Leaves are fully itemized and are the only nodes we want to look at.
    var threshold = [createState(floor,floor,floor,intellect)];
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
var talentChoices = [[0,1],[0],[0],[0],[0,1],[0,2],[0]];
//var talentCombinations = [[0,0,0,0,0,0,0]];
var talentCombinations = combineTalents(talentChoices); // 36 Combinations
console.log('Found ' + talentCombinations.length + ' talent combinations.');
var reforgeParameters1 = {budget: 30000, step: 1000, floor: 3000, ceiling: 18000, intellect: 40000}; 
var reforgeParameters2 = {budget: 27000, step: 1000, floor: 3000, ceiling: 18000, intellect: 36000};
var reforgeParameters3 = {budget: 33000, step: 1000, floor: 3000, ceiling: 18000, intellect: 44000}; 
// At each step, 500 can go 1 way, with a maximum of 17000 in any single way. Aka n^3 expansion, pruning duplicates and constraint failures.
// Floor is budget force allocated each way at least. So effective budget = budget - way*floor.
// 11000 available budget, 22 steps. 22^3 = O(10,648) reforge points, including duplicates.
var reforgeCombinations1 = combineReforge(reforgeParameters1);
var reforgeCombinations2 = combineReforge(reforgeParameters2);
var reforgeCombinations3 = combineReforge(reforgeParameters3);
var reforgeCombinations = reforgeCombinations1.concat(reforgeCombinations2).concat(reforgeCombinations3)
//var reforgeCombinations = [["c:5000,m:5000,h:5000"],["c:2000,m:5000,h:8000"],["c:5000,m:2000,h:8000"],["c:8000,m:5000,h:2000"]]
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
    Key: 'in/input-' + guid + '.txt',
    Body: jobFlowData,
    ContentType: "text/plain",
    ACL: 'public-read'
    },function (err, data) {
        if(err) {
            console.log(err);
            return;
        }
        console.log('Successfully uploaded input data: ' + 'input-' + guid + '.txt');
        emr.runJobFlow({
            Name: "Atmasim Job Flow",
            Applications: [
                {
                    Name: "Hadoop"
                },
                {
                    Name: "Hue"
                }
            ],
            Configurations: [
                {
                    Classification: "mapred-site",
                    Properties: {
                        "mapreduce.tasktracker.map.tasks.maximum": "32",
                        "mapreduce.job.reduce.slowstart.completedmaps": "1.0" // So I stop getting cucked by Reduce containers. Stop fucking shuffling jesus.
                    }
                }
            ],
            Instances: {
                Ec2SubnetId: "subnet-4b4bf203",
                Ec2KeyName: "magicsim", 
                KeepJobFlowAliveWhenNoSteps: false,
                TerminationProtected: true,
                InstanceGroups: [{
                    Name: "Master Instance Group",
                    InstanceRole: "MASTER",
                    InstanceCount: 1,
                    InstanceType: "m1.medium",
                    Market: "ON_DEMAND"
                }, {
                    Name: "Core Instance Group",
                    InstanceRole: "CORE",
                    InstanceCount: 16,
                    InstanceType: "c4.2xlarge",
                    Market: "ON_DEMAND"
                }]
            },
            JobFlowRole: "EMR_EC2_DefaultRole",
            ServiceRole: "EMR_DefaultRole",
            Steps: [{
                Name: "Copy Input to HDFS",
                ActionOnFailure: "TERMINATE_JOB_FLOW",
                HadoopJarStep: {
                    Jar: "command-runner.jar",
                    Args: [
                        "s3-dist-cp",
                        "--s3Endpoint=s3.amazonaws.com",
                        "--src=s3://atmasim/in",
                        "--dest=hdfs:///atmasim",
                        "--deleteOnSuccess"
                    ]
                }    
            },
                {
                Name: "Atmasim Driver",
                ActionOnFailure: "TERMINATE_JOB_FLOW",
                HadoopJarStep: {
                    Jar: "s3://atmasim/bin/atmasimDriver.jar",
                    Args: [
                        "hdfs:///atmasim/input-" + guid + ".txt" ,
                        "s3://atmasim/out/results-" + guid + "/",
                        numJobs + "" // Pretty dumb but I gotta cast it to a string explicitly.
                    ]
                }
            }],
            BootstrapActions: [ 
            { 
                Name: "Install SimC",
                ScriptBootstrapAction: { 
                    Path: "s3://atmasim/bin/installGate.bash"
                }
            }
            ],
            LogUri: "s3://atmasim/logs/",
            VisibleToAllUsers: false,
            ReleaseLabel: "emr-5.3.0"
        }, (err, data) => {
            if(err) {
                console.log(err);
                return;
            }
            console.log("Successfully started cluster for job: " + data.JobFlowId);
        })
});
