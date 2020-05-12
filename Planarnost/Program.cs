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
            //    { -1, 10, 0, 0, 0, 81, 73 },
            //    { 10, -1, 33, 65, 0, 43, 76 },
            //    { 0, 33, -1, 11, 12, 82, 34 },
            //    { 0, 65, 11, -1, 55, 23, 0 },
            //    { 0, 0, 12, 55, -1, 4, 90 },
            //    { 81, 43, 82, 23, 4, -1, 22 },
            //    { 73, 76, 34, 0, 90, 22, -1 }
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
