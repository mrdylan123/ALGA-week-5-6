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
            Console.WriteLine("Geef een 'x' en 'y'");

            String input = Console.ReadLine();
            string[] inputXY = input.Split(' ');
            Dungeon dungeon = new Dungeon(int.Parse(inputXY[0]), int.Parse(inputXY[1]));


            Console.ReadLine();
        }
    }
}