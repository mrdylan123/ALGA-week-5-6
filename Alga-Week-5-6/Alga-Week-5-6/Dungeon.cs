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

        private readonly int _width;
        private readonly int _height;
        private Room[,] _rooms;


        public Dungeon(int width, int height)
        {
            _width = width;
            _height = height;

            _rooms = new Room[_width, _height];

            Room previousRoom = null;

            for (int y = 0; y < _height; y++)
            {
                // Create a row of rooms
                for (int x = 0; x < _width; x++)
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
                for (int i = 0; i < _width; i++)
                {
                    Room roomAbove = _rooms[i, y - 1];
                    Room roomOnCurrentRow = _rooms[i, y];

                    Hallway hallway = new Hallway(roomOnCurrentRow, roomAbove);

                    roomAbove.AdjacentHallways.Add(hallway);
                    roomOnCurrentRow.AdjacentHallways.Add(hallway);
                }
            }

            _startRoom = _rooms[0, 0];
            _endRoom = _rooms[_width - 1, _height - 1];
            _startRoom.IsStart = true;
            _endRoom.IsEnd = true;
        }

        public void Print()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Room room = _rooms[x, y];

                    Hallway rightHallway =
                        room.AdjacentHallways.FirstOrDefault(h => h.OppositeRoom(room).X == room.X + 1);

                    Console.Write($"-{room}-{rightHallway}");
                }
                Console.Write(Environment.NewLine);
            }
            Console.ReadLine();
        }
    }
}