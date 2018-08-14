using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim.objectModels
{
    public class Consumable
    {
        public string name;
        public List<string> simc_names;

        public Consumable()
        {
            name = "";
            simc_names = new List<string>();
        }
    }
}
