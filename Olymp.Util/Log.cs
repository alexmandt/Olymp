using System;
using System.Drawing;
using Console = Colorful.Console;

namespace Olymp.Util
{
    public class Log
    {
        #region Log

        public static readonly Color Blue = Color.FromArgb(240, 6, 153);
        public static readonly Color Yellow = Color.FromArgb(247, 208, 2);
        public static readonly Color Purple = Color.FromArgb(69, 78, 158);
        public static readonly Color Red = Color.FromArgb(191, 26, 47);
        public static readonly Color Green = Color.FromArgb(1, 142, 66);
        private static readonly object Locker = new object();

        private static void Time(string node)
        {
            if (!string.IsNullOrEmpty(node)) node = $"{node} - ";
            Console.Write($"[{node}{DateTime.UtcNow:o}]", Yellow);
        }

        public static void Error(string message, string node = "")
        {
            lock (Locker)
            {
                Time(node);
                Console.WriteLine($" {message}", Red);
            }
        }

        public static void Info(string message, string node = "")
        {
            lock (Locker)
            {
                Time(node);
                Console.WriteLine($" {message}", Purple);
            }
        }

        public static void Success(string message, string node = "")
        {
            lock (Locker)
            {
                Time(node);
                Console.WriteLine($" {message}", Green);
            }
        }

        #endregion
    }
}