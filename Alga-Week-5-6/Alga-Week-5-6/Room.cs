using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Room
    {
        public ISet<Hallway> AdjacentHallways { get; set; }
        private int _x;
        private int _y;

        public Room(int x, int y)
        {
            _x = x;
            _y = y;
            AdjacentHallways = new HashSet<Hallway>();
        }

        public override string ToString()
        {
            return $"{_x},{_y}";
        }
    }
}