using System;
using System.Collections.Generic;

namespace PlanarCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph test = new Graph(7);
            //new Graph(new int[,]
            //{
            //    { -1, 0, 15, 55, 0, 51, 0 },
            //    { 0, -1, 36, 0, 97, 17, 0 },
            //    { 15, 36, -1, 89, 32, 0, 0 },
            //    { 55, 0, 89, -1, 0, 0, 76 },
            //    { 0, 97, 32, 0, -1, 79, 0 },
            //    { 51, 17, 0, 0, 79, -1, 23},
            //    { 0, 0, 0, 76, 0, 23, -1}
            //});
            Console.WriteLine(test);

            List<int[]> noPlanarGrpahIndexes = new List<int[]>();
            //if (test.CheckPlanar(noPlanarGrpahIndexes))
            //{
            //    Console.WriteLine("Планарный");
            //}
            //else
            //{
            //    Console.WriteLine("Не планарный");
            //    var table = test.MakeMinimalCoverTable(noPlanarGrpahIndexes);
            //    test.ShowMinimalCoverTable(table);
            //    test.FindAndDeleteExtraSide(table);
            //    Console.WriteLine(test);
            //    if (test.CheckPlanar(noPlanarGrpahIndexes))
            //    {
            //        Console.WriteLine("Планарный");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Не планарный");
            //    }
            //}
            while(!test.CheckPlanar(noPlanarGrpahIndexes))
            {
                Console.WriteLine("Не планарный");
                var table = test.MakeMinimalCoverTable(noPlanarGrpahIndexes);
                test.ShowMinimalCoverTable(table);
                test.FindAndDeleteExtraSide(table);
                Console.WriteLine(test);
            }

            Console.WriteLine("Планарный");
        }
    }
}
