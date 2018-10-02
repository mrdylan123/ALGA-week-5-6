using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Hallway
    {
        private int EnemyLevel { get; set; }
        private Room Entrance;
        private Room Exit;
        private Boolean isCollapsed;
        private static readonly Random rnd;

        public Hallway(Room entrance, Room exit)
        {
            EnemyLevel = rnd.Next(10);
            this.Entrance = entrance;
            this.Exit = exit;
        }

        public Room MoveThrough(Room currentRoom)
        {
            if (Entrance == currentRoom)
            {
                return Exit;
            }
            else
            {
                return Entrance;
            }
        }
    }
}