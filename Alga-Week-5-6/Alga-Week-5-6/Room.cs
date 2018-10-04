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
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }

        public Room(int x, int y)
        {
            X = x;
            Y = y;
            AdjacentHallways = new HashSet<Hallway>();
        }

        public override string ToString()
        {
            return IsStart ? "S" : IsEnd ? "E" : "X";
        }
    }
}