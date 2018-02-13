using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class PlayerResult
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Spec { get; set; }
        public string ClassReadable { get; set; }
        public string SpecReadable { get; set; }
        public double Dps { get; set; }
        public double Damage { get; set; }
        public string MainstatType { get; set; }
        public double MainstatValue { get; set; }
        public double Crit { get; set; }
        public double Haste { get; set; }
        public double Mastery { get; set; }
        public double Vers { get; set; }
        public string DpsBoost { get; set; }
    }
}
