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
                    if (matrix[i, j] == 1 || CanContrict(indexes, i, j/*, contrictedIndexesInAllMatrix*/, contrictedIndexesInRow))
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
                    noPlanarGrpahIndexes.Add((int[])indexes.Clone()) ;
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
                if(!indexes.Contains(counter) /*&& !constrictedIndexesInAllMatrix.Contains(counter)*/ && !constrictedIndexesInRow.Contains(counter))
                {
                    if (matrix[counter, i] == 1 && matrix[counter, j] == 1)
                    {
                        constrictedIndexesInRow.Add(counter);
                        return true;
                    }
                }
            }

            return false;
        }

        //public void FindUnnecessaryEdge(List<int[]> noPlanarGrpahIndexes, out int i, out int j)
        //{

        //}

        //public void ConvertToPlanar(int i, int j)
        //{

        //}

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
