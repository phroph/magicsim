using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class SimCManager
    {
        static SimC simc;
        public static SimC AcquireSimC()
        {
            if (simc != null)
            {
                return simc;
            }
            simc = new SimC();
            return simc;
        }
    }
}
