using System;
using System.Diagnostics;

namespace PlanarCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph test = //new Graph(7);
            new Graph(new int[,]
            {
                { -1, 1, 0, 0, 0, 1, 1 },
                { 1, -1, 1, 1, 0, 1, 1 },
                { 0, 1, -1, 1, 1, 1, 1 },
                { 0, 1, 1, -1, 1, 1, 0 },
                { 0, 0, 1, 1, -1, 1, 1 },
                { 1, 1, 1, 1, 1, -1, 1 },
                { 1, 1, 1, 0, 1, 1, -1 }
            });
            Console.WriteLine(test);

            if (test.CheckPlanar())
                Console.WriteLine("Планарный");
            else
                Console.WriteLine("Не планарный");
        }
    }
}
