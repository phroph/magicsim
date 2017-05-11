apt-get --assume-yes update
apt-get --assume-yes install build-essential libssl-dev unzip
wget https://github.com/simulationcraft/simc/archive/legion-dev.zip
unzip -o legion-dev.zip
cd simc-legion-dev/engine
make OPENSSL=1 optimized
p=$(pwd)
export PATH=$PATH:$p/
cd /usr/bin
ln -s $p/simc simc
