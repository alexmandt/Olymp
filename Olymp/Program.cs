using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console;
using static Olymp.Util.Log;

namespace Olymp
{
    class Program
    {
        static void Main(string[] args)
        {
            Title(0.1M);
            var startup = new Startup(args);
            startup.Configure();
            startup.Start();
        }

        private static void Title(decimal version)
        {
            Console.Title = "AthenaDB";

            foreach (var color in new List<Color>{Yellow,Green,Color.LightSkyBlue,Color.Fuchsia})
            {
                Console.Clear();
                Console.WriteAscii("AthenaDB", color);
                Console.WriteLine("The distributed document store with data-structuring capabilities", color);
                Console.WriteLine();
                Thread.Sleep(1000);
            }
        }
    }
}