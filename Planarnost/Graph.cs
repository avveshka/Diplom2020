using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanarCheck
{
    public class Graph
    {
        int[,] matrix;

        public Graph(int size)
        {
            matrix = new int[size, size];

            InitializeRandomMatrix();
        }

        public Graph(int[,] matrix)
        {
            this.matrix = matrix;
        }

        private void InitializeRandomMatrix()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i, i] = -1;
            }

            Random rnd = new Random();
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = i + 1; j < matrix.GetLength(1); j++)
                    matrix[i, j] = rnd.Next(0, 2);

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                int countUnits = 0;
                for (int j = i + 1; j < matrix.GetLength(0); j++)
                    if (matrix[i, j] != 0)
                        countUnits++;

                if (countUnits == 0)
                    matrix[i, rnd.Next(i + 1, matrix.GetLength(0))] = 1;
            }

            for (int i = 1; i < matrix.GetLength(1); i++)
            {
                int countUnits = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[j, i] != 0)
                        countUnits++;

                if (countUnits == 0)
                    matrix[rnd.Next(0, matrix.GetLength(1)), matrix.GetLength(1)] = 1;
            }

            Random rnd2 = new Random();
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = i + 1; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] == 1)
                        matrix[i, j] = rnd2.Next(1, 100);

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[j, i] = matrix[i, j];
        }

        public bool CheckPlanar(List<int[]> noPlanarGrpahIndexes)
        {
            return !(CheckOnGomeomorpfGraphAllSubGrphs(4, 5, noPlanarGrpahIndexes) | CheckOnGomeomorpfGraphAllSubGrphs(3, 6, noPlanarGrpahIndexes));
        }

        private bool CheckOnGomeomorpfGraph(int[] indexes, int expectedUnitsInRow, int expectedCountOfRows)
        {
            int numberOfUnitsInRow = 0;
            int countOfRowWithUnits = 0;
            //List<int> contrictedIndexesInAllMatrix = new List<int>();

            foreach (int i in indexes)
            {
                List<int> contrictedIndexesInRow = new List<int>();
                foreach (int j in indexes)
                {
                    if (matrix[i, j] > 0 || CanContrict(indexes, i, j/*, contrictedIndexesInAllMatrix*/, contrictedIndexesInRow))
                        numberOfUnitsInRow++;
                }

                if (numberOfUnitsInRow >= expectedUnitsInRow)
                {
                    countOfRowWithUnits++;
                    //contrictedIndexesInAllMatrix.AddRange(contrictedIndexesInRow);
                }

                numberOfUnitsInRow = 0;
            }

            if (countOfRowWithUnits == expectedCountOfRows)
                return true;

            return false;
        }

        private bool CheckOnGomeomorpfGraphAllSubGrphs(int countUnitsInRow, int coutRows, List<int[]> noPlanarGrpahIndexes)
        {
            bool isPlanar = false;
            foreach (int[] indexes in Combinations.Make(coutRows, matrix.GetLength(0)))
            {
                if (CheckOnGomeomorpfGraph(indexes, countUnitsInRow, coutRows))
                {
                    isPlanar = true;
                    noPlanarGrpahIndexes.Add((int[])indexes.Clone());
                }
            }

            return isPlanar;
        }

        private bool CanContrict(int[] indexes, int i, int j,/* List<int> constrictedIndexesInAllMatrix,*/ List<int> constrictedIndexesInRow)
        {
            if (i == j)
                return false;

            for (int counter = 0; counter < matrix.GetLength(0); counter++)
            {
                if (!indexes.Contains(counter) /*&& !constrictedIndexesInAllMatrix.Contains(counter)*/ && !constrictedIndexesInRow.Contains(counter))
                {
                    if (matrix[counter, i] > 0 && matrix[counter, j] > 0)
                    {
                        constrictedIndexesInRow.Add(counter);
                        return true;
                    }
                }
            }

            return false;
        }

        public Dictionary<int[], int[,]> MakeMinimalCoverTable(List<int[]> noPlanarGrpahIndexes)
        {
            Dictionary<int[], int[,]> resultTable = new Dictionary<int[], int[,]>();
            int constrictCounter = 0;
            List<int> contrictedIndexesInRow = new List<int>();

            foreach (int[] indexes in noPlanarGrpahIndexes)
            {
                resultTable.Add(indexes, new int[7, 7]);
            }

            foreach (int[] noPlanarGraphs in resultTable.Keys)
            {
                foreach (int[] indexes in Combinations.Make(2, matrix.GetLength(0)))
                {
                    if (noPlanarGraphs.Contains(indexes[0]) && noPlanarGraphs.Contains(indexes[1]))
                    {
                        if (matrix[indexes[0], indexes[1]] > 0)
                        {
                            resultTable[noPlanarGraphs][indexes[0], indexes[1]] = resultTable[noPlanarGraphs][indexes[1], indexes[0]] = 1;
                        }
                        else if (CanContrict(noPlanarGraphs, indexes[0], indexes[1], contrictedIndexesInRow))
                        {
                            resultTable[noPlanarGraphs][indexes[0], contrictedIndexesInRow[constrictCounter]] = resultTable[noPlanarGraphs][contrictedIndexesInRow[constrictCounter], indexes[0]] = 1;
                            resultTable[noPlanarGraphs][indexes[1], contrictedIndexesInRow[constrictCounter]] = resultTable[noPlanarGraphs][contrictedIndexesInRow[constrictCounter], indexes[1]] = 1;
                            constrictCounter++;
                        }
                    }
                }

                constrictCounter = 0;
                contrictedIndexesInRow = new List<int>();
            }

            return resultTable;
        }

        public void ShowMinimalCoverTable(Dictionary<int[], int[,]> table)
        {
            Console.Write($"indexes");
            foreach (int[] indexes in Combinations.Make(2, matrix.GetLength(0)))
            {
                Console.Write($" x{indexes[0] + 1},x{indexes[1] + 1} ");
            }
            Console.WriteLine();

            foreach (int[] indexes in table.Keys)
            {
                foreach (var item in indexes)
                {
                    Console.Write(item);
                }

                Console.Write($" ");

                foreach (int[] indexes1 in Combinations.Make(2, matrix.GetLength(0)))
                {
                    Console.Write($"   {table[indexes][indexes1[0], indexes1[1]]}   ");
                }
                Console.WriteLine();
            }
        }

        public void FindAndDeleteExtraSide(Dictionary<int[], int[,]> table)
        {
            int[] extraSide = new int[2];
            int maxCount = 0;

            foreach (int[] indexes in Combinations.Make(2, matrix.GetLength(0)))
            {
                int count = 0;
                foreach (int[] indexesAsKeys in table.Keys)
                {
                    if (table[indexesAsKeys][indexes[0], indexes[1]] == 1)
                        count++;
                }

                if (count > maxCount)
                {
                    maxCount = count;
                    extraSide = (int[])indexes.Clone();
                }
                else if (count == maxCount)
                {
                    if (matrix[indexes[0], indexes[1]] < matrix[extraSide[0], extraSide[1]])
                    {
                        extraSide = (int[])indexes.Clone();
                    }

                }
            }

            if (maxCount < table.Keys.Count)
            {
                List<int[]> otherGraphs = new List<int[]>();

                foreach (int[] indexesAsKeys in table.Keys)
                {
                    if (table[indexesAsKeys][extraSide[0], extraSide[1]] == 0)
                    {
                        otherGraphs.Add(indexesAsKeys);
                    }
                }

                FindAndDeleteExtraSide(MakeMinimalCoverTable(otherGraphs));
            }

            Console.WriteLine($"удаляем x{extraSide[0] + 1}, x{extraSide[1] + 1}");
            matrix[extraSide[0], extraSide[1]] = matrix[extraSide[1], extraSide[0]] = 0;
        }

        public override string ToString()
        {
            StringBuilder matrixInString = new StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrixInString.Append(string.Format(matrix[i, j] + "   "));

                matrixInString.Append(Environment.NewLine);
            }

            return matrixInString.ToString();
        }
    }
}
