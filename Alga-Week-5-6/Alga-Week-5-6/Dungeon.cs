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
            List<Hallway> hallwaysToScan = new List<Hallway>();
            List<Tree> roomTreeList = new List<Tree>();
            

            // Copy list of hallways into hallwaysToCollapse and hallwaysToScan
            foreach (Hallway hallway in _hallways)
            {
                hallwaysToCollapse.Add(hallway);
                hallwaysToScan.Add(hallway);
            }

            for (int i = 0; i < _hallways.Count; i++)
            {
                // Find the weakest hallway
                Hallway weakestHallway = null;

                foreach (Hallway hallway in hallwaysToScan)
                {
                    if (weakestHallway == null || hallway.EnemyLevel < weakestHallway.EnemyLevel)
                        weakestHallway = hallway;
                }

                // Check if one or both rooms have been visited from this hallway
                bool entranceVisited = visitedRooms.Contains(weakestHallway.Entrance);
                bool exitVisited = visitedRooms.Contains(weakestHallway.Exit);

                if (!entranceVisited)
                    visitedRooms.Add(weakestHallway.Entrance);

                if (!exitVisited)
                    visitedRooms.Add(weakestHallway.Exit);

                if (!entranceVisited && !exitVisited)
                {
                    // Neither have been visited, create a new tree with these two rooms
                    Tree tree = new Tree(weakestHallway.Entrance);
                    tree.AddChild(weakestHallway.Exit);
                    roomTreeList.Add(tree);
                    hallwaysToCollapse.Remove(weakestHallway);
                }
                else if (!entranceVisited)
                {
                    Tree tree = roomTreeList.First(t => t.GetChild(weakestHallway.Exit) != null).GetChild(weakestHallway.Exit);
                    tree.AddChild(weakestHallway.Entrance);
                    hallwaysToCollapse.Remove(weakestHallway);
                }
                else if (!exitVisited)
                {
                    Tree tree = roomTreeList.First(t => t.GetChild(weakestHallway.Entrance) != null).GetChild(weakestHallway.Entrance);
                    tree.AddChild(weakestHallway.Exit);
                    hallwaysToCollapse.Remove(weakestHallway);
                }
                // Both rooms have been visited, check if their trees are already connected
                else
                {
                    Tree entranceTree = roomTreeList.First(t => t.GetChild(weakestHallway.Entrance) != null);
                    Tree exitTree = roomTreeList.First(t => t.GetChild(weakestHallway.Exit) != null);
                    if (entranceTree != exitTree)
                    {
                        // The trees are not connected, so merge these and remove one from the list
                        hallwaysToCollapse.Remove(weakestHallway);
                        entranceTree.AddTree(exitTree);
                        roomTreeList.Remove(exitTree);
                    }
                }
                // Remove the hallway from the list of hallways to be scanned
                hallwaysToScan.Remove(weakestHallway);
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

        public void SilverCompass()
        {
            List<Room> rooms = new List<Room>();

            foreach (Room r in _rooms)
            {
                rooms.Add(r);
            }

            _startRoom.Distance.ShortestDistance = 0;

            while (rooms.Any())
            {
                Room shortestDistanceRoom = null;

                foreach (Room room in rooms)
                {
                    if (shortestDistanceRoom == null ||
                        room.Distance.ShortestDistance < shortestDistanceRoom.Distance.ShortestDistance)
                    {
                        shortestDistanceRoom = room;
                    }
                }

                rooms.Remove(shortestDistanceRoom);

                if (shortestDistanceRoom != _endRoom)
                {
                    IEnumerable<Hallway> adjacentHallwaysToCheck = shortestDistanceRoom.AdjacentHallways
                        .Where(h => rooms.Contains(h.OppositeRoom(shortestDistanceRoom)));

                    foreach (Hallway hallway in adjacentHallwaysToCheck)
                    {
                        int totalDistance = shortestDistanceRoom.Distance.ShortestDistance +
                                            hallway.EnemyLevel;

                        if (totalDistance < hallway.OppositeRoom(shortestDistanceRoom).Distance.ShortestDistance)
                        {
                            hallway.OppositeRoom(shortestDistanceRoom).Distance.ShortestDistance = totalDistance;
                            hallway.OppositeRoom(shortestDistanceRoom).Distance.FromRoom = shortestDistanceRoom;
                        }
                    }
                }
                else
                {

                }
            }
        }

        public void SilverCompass2()
        {
            Room currentRoom = _startRoom;
            currentRoom.Distance.ShortestDistance = 0;

            while (currentRoom != _endRoom)
            {
                Hallway weakestHallway = null;

                IEnumerable<Hallway> notVisitedHallways = currentRoom.AdjacentHallways.Where(h =>
                    h.OppositeRoom(currentRoom).Distance.ShortestDistance == int.MaxValue);

                foreach (Hallway h in notVisitedHallways)
                {
                    Room DestinationRoom = h.OppositeRoom(currentRoom);

                    // Backtrack to previously visited rooms and add up the total distance
                    int totalDistance = h.EnemyLevel;
                    Room backtrackRoom = currentRoom;

                    while (backtrackRoom.Distance.FromRoom != null)
                    {
                        totalDistance += backtrackRoom.Distance.ShortestDistance;
                        backtrackRoom = backtrackRoom.Distance.FromRoom;
                    }

                    // If a shorter path than the destination room has been found, set that distance
                    if (totalDistance < DestinationRoom.Distance.ShortestDistance)
                    {
                        DestinationRoom.Distance.FromRoom = currentRoom;
                        DestinationRoom.Distance.ShortestDistance = totalDistance;
                    }

                    if (weakestHallway == null || h.EnemyLevel < weakestHallway.EnemyLevel)
                        weakestHallway = h;
                }

                currentRoom = weakestHallway.OppositeRoom(currentRoom);
            }

            // Shortest path has been found to the endroom, now backtrack to the start so the path can be printed
            while (currentRoom != _startRoom)
            {
                currentRoom.Visited = true;
                currentRoom = currentRoom.Distance.FromRoom;
            }
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