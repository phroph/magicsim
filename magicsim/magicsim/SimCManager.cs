using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            try
            {
                simc = new SimC();
            }
            catch(Exception e)
            {
                return null;
            }
            return simc;
        }
    }
}
