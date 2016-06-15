using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo info = new DirectoryInfo("G:\\text");
            FileInfo[] files = info.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i].FullName);
            }
            string one = "hello";
            Console.WriteLine(one == "hello");
        }
    }
}
