using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Geef een 'x'");
            string x = Console.ReadLine();

            Console.WriteLine("Geef een 'y'");
            string y = Console.ReadLine();

            Dungeon dungeon = new Dungeon(int.Parse(x), int.Parse(y));

            dungeon.Print();

            string command = Console.ReadLine();

            while (command != "exit")
            {
                switch (command)
                {
                    case "talisman": dungeon.MagicTalisman();
                        break;
                    case "handgranaat": dungeon.HandGrenade();
                        break;
                    case "kompas": dungeon.SilverCompass();
                        break;
                    case "maaklastiger": dungeon.MakeShortestPathDifficult();
                        break;
                }

                try
                {
                    if (command.Contains("startx"))
                    {
                        int startX = int.Parse(command.Split(' ')[1]);
                        dungeon.SetStartLocation(x: startX);
                    }

                    if (command.Contains("starty"))
                    {
                        int startY = int.Parse(command.Split(' ')[1]);
                        dungeon.SetStartLocation(y: startY);
                    }

                    if (command.Contains("eindx"))
                    {
                        int endX = int.Parse(command.Split(' ')[1]);
                        dungeon.SetEndLocation(x: endX);
                    }

                    if (command.Contains("eindy"))
                    {
                        int endY = int.Parse(command.Split(' ')[1]);
                        dungeon.SetEndLocation(y: endY);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("De ingevoerde waarde valt buiten het speelveld.");
                }
                
                dungeon.Print();

                command = Console.ReadLine();
            }
        }
    }
}