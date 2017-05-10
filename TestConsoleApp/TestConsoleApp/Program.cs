using MetadataExtractor;
using RavuAlHemio.PSD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Regex(@"(\.jpg|jpeg|gif|bmp|png|psd)", RegexOptions.IgnoreCase);
            var s = r.Split("test.jpg.jpg");

            foreach(string str in s)
            {
                Console.WriteLine(str);
            }
            


        }
    }
}
