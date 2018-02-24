using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class ReforgePoint
    {
        public int Crit = int.MinValue;
        public int Haste = int.MinValue;
        public int Vers = int.MinValue;
        public int Mastery = int.MinValue;
    }

    public class ReforgePointEqualityComparer : IEqualityComparer<ReforgePoint>
    {
        public bool Equals(ReforgePoint x, ReforgePoint y)
        {
            return x.Crit == y.Crit && x.Haste == y.Haste && x.Vers == y.Vers && x.Mastery == y.Mastery;
        }

        public int GetHashCode(ReforgePoint obj)
        {
            var mask = 0b1111111111111111;
            return (mask & (obj.Crit - obj.Haste) << 16) & (mask & (obj.Vers - obj.Mastery));
        }
    }

    public class PlayerReforge
    {
        public string PlayerName;
        public Dictionary<ReforgePoint, double> Dps;
        public Dictionary<ReforgePoint, double> DpsError;

        public PlayerReforge(string name)
        {
            PlayerName = name;
            Dps = new Dictionary<ReforgePoint, double>(new ReforgePointEqualityComparer());
            DpsError = new Dictionary<ReforgePoint, double>(new ReforgePointEqualityComparer());
        }
    }
}
