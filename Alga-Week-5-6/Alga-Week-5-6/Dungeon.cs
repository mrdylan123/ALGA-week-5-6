using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Dungeon
    {
        private readonly Room _startRoom;
        private readonly Room _endRoom;

        private readonly int _width;
        private readonly int _height;
        private readonly Room[,] _rooms;
        private readonly List<Hallway> _hallways;


        public Dungeon(int width, int height)
        {
            _width = width;
            _height = height;

            _rooms = new Room[_width, _height];
            _hallways = new List<Hallway>();

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
                        _hallways.Add(hallway);

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
                    _hallways.Add(hallway);

                    roomAbove.AdjacentHallways.Add(hallway);
                    roomOnCurrentRow.AdjacentHallways.Add(hallway);
                }
            }

            _startRoom = _rooms[0, 0];
            _endRoom = _rooms[_width - 1, 0];
            _startRoom.IsStart = true;
            _endRoom.IsEnd = true;
        }

        public int MagicTalisman()
        {
            Queue<Room> roomQueue = new Queue<Room>();
            ISet<Room> visitedRooms = new HashSet<Room>();

            roomQueue.Enqueue(_startRoom);

            while (roomQueue.Any())
            {
                Room room = roomQueue.Dequeue();

                if (room == _endRoom)
                    break; // Found the end

                room.Visited = true;
                visitedRooms.Add(room);

                foreach (Room adjacentRoom in room.AdjacentHallways.Where(h => !h.IsCollapsed).Select(h => h.OppositeRoom(room)))
                {
                    if (!visitedRooms.Contains(adjacentRoom) && !roomQueue.Contains(adjacentRoom))
                    {
                        roomQueue.Enqueue(adjacentRoom);
                    }
                }
            }

            return visitedRooms.Count;
        }
        public void HandGrenade()
        {
            ISet<Room> visitedRooms = new HashSet<Room>();
            List<Hallway> hallwaysToCollapse = new List<Hallway>();

            // Copy list of hallways into hallwaysToCollapse
            foreach (Hallway hallway in _hallways)
            {
                hallwaysToCollapse.Add(hallway);
            }

            while (visitedRooms.Count != _width * _height)
            {
                // Find the weakest hallway
                Hallway weakestHallway = null;

                foreach (Hallway hallway in hallwaysToCollapse)
                {
                    if (weakestHallway == null || hallway.EnemyLevel < weakestHallway.EnemyLevel)
                        weakestHallway = hallway;
                }

                bool entranceVisited = visitedRooms.Contains(weakestHallway.Entrance);
                bool exitVisited = visitedRooms.Contains(weakestHallway.Exit);

                if (!entranceVisited)
                    visitedRooms.Add(weakestHallway.Entrance);

                if (!exitVisited)
                    visitedRooms.Add(weakestHallway.Exit);

                // Remove the hallway to collapse if one of the rooms has not been visited
                if (entranceVisited || exitVisited)
                    hallwaysToCollapse.Remove(weakestHallway);
            }

            foreach (Hallway hallway in hallwaysToCollapse)
            {
                hallway.IsCollapsed = true;
            }

            // Set the enemy level of a random hallway next to the startroom to 0
            Random random = new Random();
            IEnumerable<Hallway> notCollapsedHallways = _startRoom.AdjacentHallways.Where(h => !h.IsCollapsed);
            int randomDefeatedRoom = random.Next(notCollapsedHallways.Count());
            notCollapsedHallways.ElementAt(randomDefeatedRoom).EnemyLevel = 0;
        }

        public void Print()
        {
            Console.WriteLine("S = Room: Startpunt");
            Console.WriteLine("E = Room: Eindpunt");
            Console.WriteLine("X = Room: Niet bezocht");
            Console.WriteLine("* = Room: Bezocht");
            Console.WriteLine("~ = Hallway: Ingestort");
            Console.WriteLine("0 = Hallway: Level tegenstander (cost)");

            Console.Write(Environment.NewLine);

            for (int y = 0; y < _height; y++)
            {
                // Print rooms and horizontal hallways
                for (int x = 0; x < _width; x++)
                {
                    Room room = _rooms[x, y];

                    Hallway rightHallway =
                        room.AdjacentHallways.FirstOrDefault(h => h.OppositeRoom(room).X == room.X + 1);

                    Console.Write($" - {room} - {rightHallway}");
                }

                Console.Write(Environment.NewLine);

                // Print vertical hallways
                if (y == _height - 1)
                    break;

                for (int hallwayY = 0; hallwayY < 3; hallwayY++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (hallwayY == 1)
                        {
                            Room room = _rooms[x, y];

                            Hallway bottomHallway =
                                room.AdjacentHallways.FirstOrDefault(h => h.OppositeRoom(room).Y == room.Y + 1);

                            Console.Write($"   {bottomHallway}    ");
                        }
                        else
                        {
                            Console.Write("   |    ");
                        }
                    }

                    Console.Write(Environment.NewLine);
                }
            }
            Console.ReadLine();
        }
    }
}