#!/bin/bash

# Credit for python script: adam_vandenberg https://forums.aws.amazon.com/thread.jspa?threadID=222418

python - <<'__SCRIPT__'
import sys
import json
 
instance_file = "/mnt/var/lib/info/instance.json"
is_master = False
try:
    with open(instance_file) as f:
        props = json.load(f)
 
    is_master = props.get('isMaster', False)
except IOError as ex:
    pass # file will not exist when testing on a non-emr machine
 
if is_master:
    sys.exit(0)
else:
    sys.exit(1)
__SCRIPT__

if [ $? -eq 1 ] 
then
    sudo yum -y update
    sudo yum -y groups mark install "Development Tools" "Development Libraries"
    sudo yum -y groups mark convert "Development Tools" "Development Libraries"
    sudo yum -y groupinstall "Development Tools" "Development Libraries"
    sudo yum -y install openssl-devel
    wget https://github.com/simulationcraft/simc/archive/legion-dev.zip
    unzip -o legion-dev.zip
    cd simc-legion-dev/engine
    make OPENSSL=1 optimized
    p=$(pwd)
    export PATH=$PATH:$p/
    cd /usr/bin
    sudo ln -s $p/simc simc
    exit 0
else
    exit 0
fi