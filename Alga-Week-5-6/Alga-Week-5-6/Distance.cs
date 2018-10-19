using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Distance
    {
        public int ShortestDistance { get; set; }
        public Room FromRoom { get; set; }

        public Distance()
        {
            ShortestDistance = int.MaxValue;
        } 
    }
}
