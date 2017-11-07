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
    var hfloor = params.hfloor;
    var intellect = params.intellect;
    var ceiling = params.ceiling;
    var hceiling = params.hceiling;
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
                if(this.haste+stepSize<=hceiling) {
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
    var threshold = [createState(floor,floor,hfloor,intellect)];
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

var combineLegendaries = function(params) {
    var legendaries = [];
    for(var i=0; i<params.length; i++) {
        for(var j=i+1; j<params.length; j++) {
            legendaries.push(params[i]+";"+params[j]);
        }
    }
    return legendaries;
}

/*var combineRelics = function(params) {
    var relics = [];
    
    var usedRelicMapping = {};
    return relics;
}*/

var combineCrucible = function(params) {

}

var simModel = require('../models.js').models[0]; // ToS
var simCombinations = combineSims(simModel); // 28 Combinations
console.log('Found ' + simCombinations.length + ' sim combinations.');
var talentChoices = [[1],[1],[1],[1],[2],[3],[1]];
var sothpTalentChoices = [[2],[1],[1],[1],[2],[3],[1]];
//var talentCombinations = [[0,0,0,0,0,0,0]];
var talentCombinations = combineTalents(talentChoices);
var sothpTalentCombinations = combineTalents(sothpTalentChoices);
console.log('Found ' + talentCombinations.length + ' talent combinations.');
var acridReforgeParameters1 = {budget: 38000, step: 1000, floor: 3000, hceiling: 20000, hfloor: 8000, ceiling: 18000, intellect: 65000};
var reforgeParameters1 = {budget: 42000, step: 1000, floor: 3000, hceiling: 20000, hfloor: 8000, ceiling: 18000, intellect: 61000}; 
var acridReforgeParameters2 = {budget: 36000, step: 1000, floor: 3000, hceiling: 20000, hfloor:8000, ceiling: 18000, intellect: 56000};
var reforgeParameters2 = {budget: 40000, step: 1000, floor: 3000, hceiling: 20000, hfloor: 8000, ceiling: 18000, intellect: 53000};
// +1 intellect to put it into a different int key. Soul has vastly different secondary stats, which skew it's value significantly.
// It's closer to 1.7k but 2k has to be a round approximation for 1k step size.
var soulAcridReforgeParameters1 = {budget: 36000, step: 1000, floor: 3000, hceiling: 20000, hfloor: 8000, ceiling: 18000, intellect: 65001};
var soulReforgeParameters1 = {budget: 40000, step: 1000, floor: 3000, hceiling: 20000, hfloor: 8000, ceiling: 18000, intellect: 61001}; 
var soulAcridReforgeParameters2 = {budget: 34000, step: 1000, floor: 3000, hceiling: 20000, hfloor:8000, ceiling: 18000, intellect: 56001};
var soulReforgeParameters2 = {budget: 38000, step: 1000, floor: 3000, hceiling: 20000, hfloor: 8000, ceiling: 18000, intellect: 53001};  
var legendaryParameters = ["sephuz","mangaza"]//,"shahraz","zeks"]; // Soul has to be added separately because of talent issues.
//var legendaryCombinations = combineLegendaries(legendaryParameters);
var legendaryCombinations = ["mangaza;sephuz","mangaza;shahraz","mangaza;zeks"];
console.log('Found ' + legendaryCombinations.length + ' legendary combinations.');
// Take exactly 6, where a maximum of 3 from any given trait.
var relicParameters = [779,778];
var relicCombinations = ["1573:775:775:775:775:775","1573:1573:775:775:775:775","1573:1573:1573:775:775:775","775:775:775:775:775:775"];
console.log('Found ' + relicCombinations.length + ' relic combinations.');
// 1739:3 is always required.
var crucibleParameters = [1780,1778,1779,1777,1770,1782,1783];
var crucibleCombinations = ["1739:3"];
console.log('Found ' + crucibleCombinations.length + ' crucible combinations.');
// At each step, 500 can go 1 way, with a maximum of 17000 in any single way. Aka n^3 expansion, pruning duplicates and constraint failures.
// Floor is budget force allocated each way at least. So effective budget = budget - way*floor.
// 11000 available budget, 22 steps. 22^3 = O(10,648) reforge points, including duplicates.

// Expecting 84 crucible combinations.
// Expecting 56 trait combinations.

var reforge = true;
if(reforge) {
    var reforgeCombinations1 = combineReforge(reforgeParameters1);
    var reforgeCombinations2 = combineReforge(reforgeParameters2);
    var acridReforgeCombinations1 = combineReforge(acridReforgeParameters1);
    var acridReforgeCombinations2 = combineReforge(acridReforgeParameters2);
    var soulReforgeCombinations1 = combineReforge(soulReforgeParameters1);
    var soulReforgeCombinations2 = combineReforge(soulReforgeParameters2);
    var soulAcridReforgeCombinations1 = combineReforge(soulAcridReforgeParameters1);
    var soulAcridReforgeCombinations2 = combineReforge(soulAcridReforgeParameters2);
    //var reforgeCombinations3 = combineReforge(reforgeParameters3);
    var reforgeCombinations = reforgeCombinations1.concat(reforgeCombinations2)//.concat(reforgeCombinations3)
    var acridReforgeCombinations = acridReforgeCombinations1.concat(acridReforgeCombinations2)//.concat(reforgeCombinations3)
    var soulReforgeCombinations = soulReforgeCombinations1.concat(soulReforgeCombinations2)//.concat(reforgeCombinations3)
    var soulAcridReforgeCombinations = soulAcridReforgeCombinations1.concat(soulAcridReforgeCombinations2)//.concat(reforgeCombinations3)
    //var reforgeCombinations = reforgeCombinations3
    //var reforgeCombinations = [["c:5000,m:5000,h:5000"],["c:2000,m:5000,h:8000"],["c:5000,m:2000,h:8000"],["c:8000,m:5000,h:2000"]]
    console.log('Found ' + (reforgeCombinations.length + acridReforgeCombinations.length) + ' reforge combinations.');

    // Now shit gets real. We take the cartesian product of all 3 of these basically. And line-by-line add records into jobFlowData.

    var numJobs = 0;

    // Add base info
    /*
    simCombinations.forEach((sim) => {
        talentCombinations.forEach((talent) => {
            reforgeCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                    legendaryCombinations.forEach((legendaries) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';' + legendaries + ';' + crucible + ';false\n';
                            numJobs++;
                        })
                    })
                })
            })
        })
    })
    simCombinations.forEach((sim) => {
        talentCombinations.forEach((talent) => {
            acridReforgeCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                    legendaryCombinations.forEach((legendaries) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';' + legendaries + ';' + crucible + ';true\n';
                            numJobs++;
                        })
                    })
                })
            })
        })
    })*/

    // Soul of the High Priest
    simCombinations.forEach((sim) => {
        sothpTalentCombinations.forEach((talent) => {
            soulReforgeCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';mangaza;soul;' + crucible + ';false\n';
                            numJobs++;
                        })
                    })
                })
        })
    })
    simCombinations.forEach((sim) => {
        sothpTalentCombinations.forEach((talent) => {
            soulAcridReforgeCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';mangaza;soul;' + crucible + ';true\n';
                            numJobs++;
                        })
                })
            })
        })
    })

    // No Legendary
    simCombinations.forEach((sim) => {
        talentCombinations.forEach((talent) => {
            reforgeCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';none;none;' + crucible + ';false\n';
                            numJobs++;
                        })
                    })
                })
        })
    })
    simCombinations.forEach((sim) => {
        talentCombinations.forEach((talent) => {
            acridReforgeCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';none;none;' + crucible + ';true\n';
                            numJobs++;
                        })
                })
            })
        })
    })

}
else {
    var gearCombinations = [];
    var gearParameters = {
        template: "head=,id=%HID%,ilevel=%HILVL%%NL%neck=,id=%NID%,ilevel=%NLVL%,enchant=%NENCH%%NL%shoulders=,id=%SID%,ilevel=%SILVL%%NL%back=,id=%BID%,ilevel=%BILVL,enchant=%BENCH%%NL%chest=,id=%CID%,ilevel=%CILVL%%NL%wrists=,id=%WID%,ilevel=%WILVL%%NL%hands=,id=%HAID%,ilevel=%HAILVL%%NL%waist=,id=%WAID%,ilevel=%WAILVL%%NL%legs=,id=%LID%,ilevel=%LILVL%%NL%feet=,id=%FID%,ilevel=%FILVL%%NL%finger1=,id=%F1ID%,ilevel=%F1ILVL%,enchant=%F1ENCH%%NL%finger2=,id=%F2ID%,ilevel=%F2ILVL%,enchant=%F2ENCH%%NL%trinket1=,id=%T1ID%,ilevel=%T1ILVL%%NL%trinket2=,id=%T2ID%,ilevel=%T2ILVL%%NL%main_hand=,id=128827,bonus_id=740,gem_id=%R1ID%/%R2ID%/%R3ID%,relic_ilevel=%R1ILVL%/%R2ILVL%/%R3ILVL%%NL%off_hand=,id=133958%NL%",
        baseGear: {
            H: {id:"147165",lvl:930},
            N: {id:"147013",lvl:930},
            S: {id:"146997",lvl:930},
            B: {id:"147163",lvl:930},
            C: {id:"147167",lvl:930},
            W: {id:"147000",lvl:930},
            HA: {id:"146988",lvl:940},
            WA: {id:"146999",lvl:930},
            L: {id:"147166",lvl:930},
            F: {id:"146987",lvl:930},
            F1: {id:"147195",lvl:940},
            F2: {id:"147020",lvl:930},
            T2: {id:"142507",lvl:930}
        },
        modifications: [
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
            [{slot:"T1",id:""}],
        ],
        modificationReforgeParameters: {
            min: 850,
            max: 955,
            stepSize: 5
        },
        tierCombinations: [
            ["tier20_2pc=1","tier20_4pc=1"]
        ]
    };
    gearCombinations = combineGear(gearParameters);

    simCombinations.forEach((sim) => {
        talentCombinations.forEach((talent) => {
            gearCombinations.forEach((gear) => {
                relicCombinations.forEach((relic) => {
                    legendaryCombinations.forEach((legendaries) => {
                        crucibleCombinations.forEach((crucible) => {
                            jobFlowData += sim + ';' + talent + ';' + gear + ';' + relic + ';' + legendaries + ';' + crucible + '\n';
                            numJobs++;
                        })
                    })
                })
            })
        })
    })
}
console.log('Found ' + numJobs + ' jobs.');

var instances = 16;

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
                },
                {
                    Name: "Ganglia"
                }
            ],
            Configurations: [
                {
                    Classification: "mapred-site",
                    Properties: {
                        "mapred.tasktracker.map.tasks.maximum": "2",
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
                    InstanceCount: instances,
                    InstanceType: "c4.8xlarge",
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
                        numJobs + "", // Pretty dumb but I gotta cast it to a string explicitly.
                        (36*instances) + ""
                    ]
                }
            }],
            BootstrapActions: [ 
            { 
                Name: "Install SimC",
                ScriptBootstrapAction: { 
                    Path: "s3://atmasim/bin/installGate.bash"
                }
            },
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
