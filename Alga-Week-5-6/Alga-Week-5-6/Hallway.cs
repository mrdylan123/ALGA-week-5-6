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
        private readonly Room _entrance;
        private readonly Room _exit;
        private bool _isCollapsed;

        private static readonly Random random = new Random();

        public Hallway(Room entrance, Room exit)
        {
            EnemyLevel = random.Next(9);
            this._entrance = entrance;
            this._exit = exit;
        }

        public Room OppositeRoom(Room currentRoom)
        {
            return _entrance == currentRoom ? _exit : _entrance;
        }

        public override string ToString()
        {
            return EnemyLevel.ToString();
        }
    }
}