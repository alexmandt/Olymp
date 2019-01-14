using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Colorful;
using static Olymp.Util.Log;

namespace Olymp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // Show version and set title
            await Title(0.1M);
            // Configure the application with the arguments provided
            var startup = new Startup(args);

            // Start the application
            startup.Start();
        }

        private static async Task Title(decimal version)
        {
            // Set the title
            Console.Title = "Olymp";

            // Show colorfull version info
            foreach (var color in new List<Color> {Yellow, Green, Color.LightSkyBlue, Color.Fuchsia})
            {
                Console.Clear();
                Console.WriteAscii($"Olymp v{version}", color);
                Console.WriteLine("The simple distributed compute engine", color);
                Console.WriteLine();
                await Task.Delay(500);
            }
        }
    }
}