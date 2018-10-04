using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Dungeon
    {
        private Room _startRoom;
        private Room _endRoom;

        private Room[,] _rooms;


        public Dungeon(int width, int height)
        {
            _rooms = new Room[width, height];

            Room previousRoom = null;

            for (int y = 0; y < height; y++)
            {
                // Create a row of rooms
                for (int x = 0; x < width; x++)
                {
                    Room room = new Room(x, y);
                    _rooms[x, y] = room;

                    // Link rooms horizontally with hallways
                    if (previousRoom != null)
                    {
                        Hallway hallway = new Hallway(previousRoom, room);
                        previousRoom.AdjacentHallways.Add(hallway);
                        room.AdjacentHallways.Add(hallway);
                    }

                    previousRoom = room;
                }

                previousRoom = null;

                if (y == 0) continue;

                // Link the current row of rooms with the rooms above
                for (int i = 0; i < width; i++)
                {
                    Room roomAbove = _rooms[i, y - 1];
                    Room roomOnCurrentRow = _rooms[i, y];

                    Hallway hallway = new Hallway(roomOnCurrentRow, roomAbove);

                    roomAbove.AdjacentHallways.Add(hallway);
                    roomOnCurrentRow.AdjacentHallways.Add(hallway);
                }
            }

            _startRoom = _rooms[0, 0];
            _endRoom = _rooms[width - 1, height - 1];
        }


    }
}