using System;
using System.Collections.Generic;
using System.IO;

namespace Olymp.Util
{
    public class FileHelper
    {
        public static string ReadHexString(string f)
        {
            var fs = new FileStream(f, FileMode.Open);
            int hexIn;
            var hex = "";

            for (var i = 0; (hexIn = fs.ReadByte()) != -1; i++) hex += string.Format("{0:X2}", hexIn);

            return hex;
        }

        public static void WriteHexString(string c, string outputPath)
        {
            var byteList = new List<byte>();
            for (var i = 0; i < c.Length; i++)
            {
                var data = Convert.ToInt32($"0x{c[i]}{c[i + 1]}", 16);
                byteList.Add((byte) data);
                i++;
            }

            File.WriteAllBytes(outputPath, byteList.ToArray());
        }
    }
}