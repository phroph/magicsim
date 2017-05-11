/*{
   "AdditionalInfo": "string",
   "AmiVersion": "string",
   "Applications": [ 
      { 
         "AdditionalInfo": { 
            "string" : "string" 
         },
         "Args": [ "string" ],
         "Name": "string",
         "Version": "string"
      }
   ],
   "AutoScalingRole": "string",
   "BootstrapActions": [ 
      { 
         "Name": "string",
         "ScriptBootstrapAction": { 
            "Args": [ "string" ],
            "Path": "string"
         }
      }
   ],
   "Configurations": [ 
      { 
         "Classification": "string",
         "Configurations": [ 
            "Configuration"
         ],
         "Properties": { 
            "string" : "string" 
         }
      }
   ],
   "Instances": { 
      "AdditionalMasterSecurityGroups": [ "string" ],
      "AdditionalSlaveSecurityGroups": [ "string" ],
      "Ec2KeyName": "string",
      "Ec2SubnetId": "string",
      "Ec2SubnetIds": [ "string" ],
      "EmrManagedMasterSecurityGroup": "string",
      "EmrManagedSlaveSecurityGroup": "string",
      "HadoopVersion": "string",
      "InstanceCount": number,
      "InstanceFleets": [ 
         { 
            "InstanceFleetType": "string",
            "InstanceTypeConfigs": [ 
               { 
                  "BidPrice": "string",
                  "BidPriceAsPercentageOfOnDemandPrice": number,
                  "Configurations": [ 
                     { 
                        "Classification": "string",
                        "Configurations": [ 
                           "Configuration"
                        ],
                        "Properties": { 
                           "string" : "string" 
                        }
                     }
                  ],
                  "EbsConfiguration": { 
                     "EbsBlockDeviceConfigs": [ 
                        { 
                           "VolumeSpecification": { 
                              "Iops": number,
                              "SizeInGB": number,
                              "VolumeType": "string"
                           },
                           "VolumesPerInstance": number
                        }
                     ],
                     "EbsOptimized": boolean
                  },
                  "InstanceType": "string",
                  "WeightedCapacity": number
               }
            ],
            "LaunchSpecifications": { 
               "SpotSpecification": { 
                  "BlockDurationMinutes": number,
                  "TimeoutAction": "string",
                  "TimeoutDurationMinutes": number
               }
            },
            "Name": "string",
            "TargetOnDemandCapacity": number,
            "TargetSpotCapacity": number
         }
      ],
      "InstanceGroups": [ 
         { 
            "AutoScalingPolicy": { 
               "Constraints": { 
                  "MaxCapacity": number,
                  "MinCapacity": number
               },
               "Rules": [ 
                  { 
                     "Action": { 
                        "Market": "string",
                        "SimpleScalingPolicyConfiguration": { 
                           "AdjustmentType": "string",
                           "CoolDown": number,
                           "ScalingAdjustment": number
                        }
                     },
                     "Description": "string",
                     "Name": "string",
                     "Trigger": { 
                        "CloudWatchAlarmDefinition": { 
                           "ComparisonOperator": "string",
                           "Dimensions": [ 
                              { 
                                 "Key": "string",
                                 "Value": "string"
                              }
                           ],
                           "EvaluationPeriods": number,
                           "MetricName": "string",
                           "Namespace": "string",
                           "Period": number,
                           "Statistic": "string",
                           "Threshold": number,
                           "Unit": "string"
                        }
                     }
                  }
               ]
            },
            "BidPrice": "string",
            "Configurations": [ 
               { 
                  "Classification": "string",
                  "Configurations": [ 
                     "Configuration"
                  ],
                  "Properties": { 
                     "string" : "string" 
                  }
               }
            ],
            "EbsConfiguration": { 
               "EbsBlockDeviceConfigs": [ 
                  { 
                     "VolumeSpecification": { 
                        "Iops": number,
                        "SizeInGB": number,
                        "VolumeType": "string"
                     },
                     "VolumesPerInstance": number
                  }
               ],
               "EbsOptimized": boolean
            },
            "InstanceCount": number,
            "InstanceRole": "string",
            "InstanceType": "string",
            "Market": "string",
            "Name": "string"
         }
      ],
      "KeepJobFlowAliveWhenNoSteps": boolean,
      "MasterInstanceType": "string",
      "Placement": { 
         "AvailabilityZone": "string",
         "AvailabilityZones": [ "string" ]
      },
      "ServiceAccessSecurityGroup": "string",
      "SlaveInstanceType": "string",
      "TerminationProtected": boolean
   },
   "JobFlowRole": "string",
   "LogUri": "string",
   "Name": "string",
   "NewSupportedProducts": [ 
      { 
         "Args": [ "string" ],
         "Name": "string"
      }
   ],
   "ReleaseLabel": "string",
   "ScaleDownBehavior": "string",
   "SecurityConfiguration": "string",
   "ServiceRole": "string",
   "Steps": [ 
      { 
         "ActionOnFailure": "string",
         "HadoopJarStep": { 
            "Args": [ "string" ],
            "Jar": "string",
            "MainClass": "string",
            "Properties": [ 
               { 
                  "Key": "string",
                  "Value": "string"
               }
            ]
         },
         "Name": "string"
      }
   ],
   "SupportedProducts": [ "string" ],
   "Tags": [ 
      { 
         "Key": "string",
         "Value": "string"
      }
   ],
   "VisibleToAllUsers": boolean
}





POST / HTTP/1.1
Content-Type: application/x-amz-json-1.1
X-Amz-Target: ElasticMapReduce.RunJobFlow
Content-Length: 734
User-Agent: aws-sdk-ruby/1.9.2 ruby/1.9.3 i386-mingw32
Host: us-east-1.elasticmapreduce.amazonaws.com
X-Amz-Date: 20130715T210803Z
X-Amz-Content-Sha256: 8676d21986e4628a89fb1232a1344063778d4ffc23d10be02b437e0d53a24db3
Authorization: AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130715/us-east-1/elasticmapreduce/aws4_request, SignedHeaders=content-length;content-type;host;user-agent;x-amz-content-sha256;x-amz-date;x-amz-target, Signature=71f79725c4dbe77c0e842718485f0b37fe6df69e1153c80f7748ebd9617ca2f3
Accept:


{
    "Name": "Atmasim Job Flow",
    "Instances": {
        "KeepJobFlowAliveWhenNoSteps": "false",
        "TerminationProtected": "false",
        "InstanceGroups": [{
            "Name": "Master Instance Group",
            "InstanceRole": "MASTER",
            "InstanceCount": 1,
            "InstanceType": "c4.8xlarge",
            "Market": "ON_DEMAND"
        }],
        "InstanceGroups": [{
            "Name": "Core Instance Group",
            "InstanceRole": "CORE",
            "InstanceCount": 8,
            "InstanceType": "c4.8xlarge",
            "Market": "ON_DEMAND"
        }]
    },
    "Steps": [{
        "Name": "Atmasim Driver",
        "ActionOnFailure": "CANCEL_AND_WAIT",
        "HadoopJarStep": {
            "Jar": "s3://atmasim/atmasimDriver.jar",
            "Args": [
                "s3://atmasim/atmasimInput.dat",
                "s3://atmasim/results"]
            ]
        }
    }],
    "BootstrapActions": [ 
       { 
          "Name": "Install SimC",
          "ScriptBootstrapAction": { 
             "Path": "s3://atmasim/installSimC.sh"
          }
       }
    ],
    "VisibleToAllUsers": "false",
    "NewSupportedProduct": [],
    "AmiVersion": "5.4.0"
}







*/