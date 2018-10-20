using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Hallway
    {
        public int EnemyLevel { get; set; }
        public Room Entrance { get; set; }
        public Room Exit { get; set; }
        public bool IsCollapsed { get; set; }

        private static readonly Random random = new Random();

        public Hallway(Room entrance, Room exit)
        {
            EnemyLevel = random.Next(9);
            this.Entrance = entrance;
            this.Exit = exit;
        }

        public Room OppositeRoom(Room currentRoom)
        {
            return Entrance == currentRoom ? Exit : Entrance;
        }

        public override string ToString()
        {
            return IsCollapsed ? "~" : EnemyLevel.ToString();
        }
    }
}