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
        private Room _startRoom;
        private Room _endRoom;

        private readonly int _width;
        private readonly int _height;
        private readonly Room[,] _rooms;
        private readonly List<Hallway> _hallways;
        private readonly Stack<Room> _shortestPath;


        public Dungeon(int width, int height)
        {
            _width = width;
            _height = height;

            _rooms = new Room[_width, _height];
            _hallways = new List<Hallway>();
            _shortestPath = new Stack<Room>();

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
            _endRoom = _rooms[_width - 1, _height - 1];
            _startRoom.IsStart = true;
            _endRoom.IsEnd = true;
        }

        public void SetStartLocation(int? x = null, int? y = null)
        {
            Room previousStartRoom = _startRoom;

            if (x != null)
                _startRoom = _rooms[x.Value, _startRoom.Y];

            if (y != null)
                _startRoom = _rooms[_startRoom.X, y.Value];

            previousStartRoom.IsStart = false;
            _startRoom.IsStart = true;
        }

        public void SetEndLocation(int? x = null, int? y = null)
        {
            Room previousEndRoom = _endRoom;
            
            if (x != null)
                _endRoom = _rooms[x.Value, _endRoom.Y];

            if (y != null)
                _endRoom = _rooms[_endRoom.X, y.Value];

            previousEndRoom.IsEnd = false;
            _endRoom.IsEnd = true;
        }

        public void MagicTalisman()
        {
            Queue<Room> roomQueue = new Queue<Room>();
            ISet<Room> visitedRooms = new HashSet<Room>();

            roomQueue.Enqueue(_startRoom);

            while (roomQueue.Any())
            {
                Room room = roomQueue.Dequeue();

                if (room == _endRoom)
                    break; // Found the end

                visitedRooms.Add(room);

                // Add not visited adjacent rooms to the queue
                foreach (Room adjacentRoom in room.AdjacentHallways.Where(h => !h.IsCollapsed).Select(h => h.OppositeRoom(room)))
                {
                    if (!visitedRooms.Contains(adjacentRoom) && !roomQueue.Contains(adjacentRoom))
                    {
                        adjacentRoom.Distance.FromRoom = room;
                        roomQueue.Enqueue(adjacentRoom);
                    }
                }
            }

            int stepCount = 0;

            // Go from the end room to the start room while counting the amount of steps
            Room roomInPath = _endRoom;

            while (roomInPath.Distance.FromRoom != null)
            {
                roomInPath = roomInPath.Distance.FromRoom;
                stepCount++;
            }

            Reset();

            Console.WriteLine($"De talisman licht op en fluistert dat het eindpunt {stepCount} stap(pen) ver weg is" + Environment.NewLine);
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
                    CreateRoomTree(weakestHallway.Entrance, weakestHallway.Exit, roomTreeList);
                    hallwaysToCollapse.Remove(weakestHallway);
                }
                else if (!entranceVisited)
                {
                    // Exit has been visited, add the entrance to the exit's tree
                    AddRoomToRoomTree(weakestHallway.Entrance, weakestHallway.Exit, roomTreeList);
                    hallwaysToCollapse.Remove(weakestHallway);
                }
                else if (!exitVisited)
                {
                    // Entrance has been visited, add the exit to the entrance's tree
                    AddRoomToRoomTree(weakestHallway.Exit, weakestHallway.Entrance, roomTreeList);
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
                        MergeRoomTrees(entranceTree, exitTree, roomTreeList);
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

            Console.WriteLine("De kerker schudt op zijn grondvesten, de tegenstander in een aangrenzende hallway is vermorzeld! " +
                              "Een donderend geluid maakt duidelijk dat gedeeltes van de kerker zijn ingestort..." + Environment.NewLine);
        }

        private void CreateRoomTree(Room room1, Room room2, List<Tree> roomTreeList)
        {
            Tree tree = new Tree(room1);
            tree.AddChild(room2);
            roomTreeList.Add(tree);
        }

        private void AddRoomToRoomTree(Room room, Room roomInTree, List<Tree> roomTreeList)
        {
            Tree tree = roomTreeList.First(t => t.GetChild(roomInTree) != null).GetChild(roomInTree);
            tree.AddChild(room);
        }

        private void MergeRoomTrees(Tree tree1, Tree tree2, List<Tree> roomTreeList)
        {
            tree1.AddTree(tree2);
            roomTreeList.Remove(tree2);
        }

        public void SilverCompass()
        {
            Reset();

            List<Room> rooms = new List<Room>();

            foreach (Room r in _rooms)
            {
                rooms.Add(r);
            }

            _startRoom.Distance.ShortestDistance = 0;

            while (rooms.Any())
            {
                // Find the room that has the shortest total distance
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
                    // Update the shortest distance for every adjacent room of the shortestDistanceRoom
                    IEnumerable<Hallway> adjacentHallwaysToCheck = shortestDistanceRoom.AdjacentHallways
                        .Where(h => !h.IsCollapsed && rooms.Contains(h.OppositeRoom(shortestDistanceRoom)));

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
                    // End room has been found. Backtrack the shortest path and add it the the shortestPath stack
                    shortestDistanceRoom = _endRoom;

                    if (shortestDistanceRoom.Distance.FromRoom != null || shortestDistanceRoom == _startRoom)
                    {
                        while (shortestDistanceRoom != null)
                        {
                            _shortestPath.Push(shortestDistanceRoom);
                            shortestDistanceRoom = shortestDistanceRoom.Distance.FromRoom;
                        }
                    }

                    PrintShortestPath();
                }
            }
        }

        private void PrintShortestPath()
        {
            Console.WriteLine("Je haalt het kompas uit je zak. Het trilt in je hand en projecteert in lichtgevende letters op de muur: ");

            Room previousRoom = null;

            int shortestPathLength = _shortestPath.Count;

            foreach (Room room in _shortestPath) {
                if (previousRoom != null) {
                    if (_shortestPath.Count - 1 != shortestPathLength)
                        Console.Write(" - ");

                    Console.Write(GetDirection(previousRoom, room));
                }

                room.Visited = true;

                previousRoom = room;

                shortestPathLength--;
            }

            Console.WriteLine(Environment.NewLine);

            previousRoom = null;

            shortestPathLength = _shortestPath.Count;

            Console.Write($"{_shortestPath.Count - 1} tegenstanders (");

            foreach (Room room in _shortestPath) {
                if (previousRoom != null) {
                    if (_shortestPath.Count - 1 != shortestPathLength)
                        Console.Write(", ");

                    Console.Write(GetEnemyLevel(room, previousRoom));
                }

                previousRoom = room;

                shortestPathLength--;
            }

            Console.Write(")" + Environment.NewLine + Environment.NewLine);
        }

        private string GetEnemyLevel(Room from, Room to)
        {
            Hallway hallway = from.AdjacentHallways.First(h => h.OppositeRoom(from) == to);

            return $"level {hallway.EnemyLevel}";
        }

        private string GetDirection(Room from, Room to)
        {
            int xDirection = to.X - from.X;
            int yDirection = to.Y - from.Y;

            if (xDirection == 1)
                return "Oost";

            if (xDirection == -1)
                return "West";

            if (yDirection == 1)
                return "Zuid";

            if (yDirection == -1)
                return "Noord";

            return null;
        }

        public void MakeShortestPathDifficult()
        {
            Room from = null;

            // For every room in the shortest path, raise the enemy level by 3 (with a maximum of 10)
            foreach (Room room in _shortestPath)
            {
                if (from != null)
                {
                    Hallway hallway = from.AdjacentHallways.First(h => h.OppositeRoom(from) == room);

                    hallway.EnemyLevel += 3;

                    if (hallway.EnemyLevel >= 10)
                        hallway.EnemyLevel = 10;
                }

                from = room;
            }

            Reset();
        }

        private void Reset()
        {
            foreach (Room room in _rooms) {
                room.Reset();
            }

            _shortestPath.Clear();
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

            Console.WriteLine(Environment.NewLine + "Acties: talisman, handgranaat, kompas, maaklastiger, startx [x positie], starty [y positie], eindx [x positie], eindy [y positie]" + Environment.NewLine);
        }
    }
}