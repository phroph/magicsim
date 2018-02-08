using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class SimDataManager
    {
        private static SimData _simData;

        public static SimData GetSimData()
        {
            if(_simData == null)
            {
                _simData = new SimData();
            }
            return _simData;
        }

        public static void ResetSimData()
        {
            _simData = new SimData();
        }
    }
}
