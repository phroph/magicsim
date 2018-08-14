using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim.objectModels
{
    public class Model
    {
        public string dispName;
        public string name;
        public Dictionary<string, float> model;
        public Dictionary<string, float> timeModel;

        public Model()
        {
            model = new Dictionary<string, float>();
            timeModel = new Dictionary<string, float>();
        }
    }
}
