#!/bin/sh
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