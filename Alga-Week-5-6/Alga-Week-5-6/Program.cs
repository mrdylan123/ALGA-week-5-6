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

            //dungeon.HandGrenade();

            dungeon.SilverCompass();
            //Console.WriteLine($"Aantal stappen naar het einde: {dungeon.MagicTalisman()}");

            dungeon.Print();
        }
    }
}