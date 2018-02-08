using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class SimNode
    {
        public List<Player> players;
    }
    
    public class Player
    {
        public string name;
        public string specialization;
        public ScaleFactors scale_factors;
        public CollectedData collected_data;
    }

    public class ScaleFactors
    {
        public double Int;
        public double Agi;
        public double Str;
        public double Crit;
        public double Haste;
        public double Mastery;
        public double Vers;
    }

    public class CollectedData
    {
        public Dps dps;
        public Damage dmg;
    }

    public class Dps
    {
        public double mean;
    }

    public class Damage
    {
        public double mean;
    }
}
