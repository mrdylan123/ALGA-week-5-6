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

        public Hallway(int level, Room entrance, Room exit)
        {
            this.EnemyLevel = level;
            this.Entrance = entrance;
            this.Exit = exit;
        }


    }
}