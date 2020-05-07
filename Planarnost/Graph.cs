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

        public bool CheckPlanar()
        {
            return !(CheckOnK5AllSubGrphs() || CheckOnK33AllSubGrphs());
        }

        private bool CheckOnK5(int[] indexes)
        {
            int numberOfUnitsInRow = 0;
            int countOfRowWithFourUnits = 0;
            
            foreach(int i in indexes)
            {
                List<int> contrictedIndexes = new List<int>();
                foreach (int j in indexes)
                {
                    if (matrix[i, j] == 1 || CanConstrict(indexes, i, j, contrictedIndexes))
                        numberOfUnitsInRow++;
                }

                if (numberOfUnitsInRow == 4)
                    countOfRowWithFourUnits++;

                numberOfUnitsInRow = 0;
            }

            if (countOfRowWithFourUnits == 5)
                return true;

            return false;
        }

        private bool CheckOnK5AllSubGrphs()
        {
            foreach (int[] indexes in Combinations.Make(5, matrix.GetLength(0)))
                if (CheckOnK5(indexes))
                    return true;

            return false;
        }

        private bool CheckOnK33(int[] indexes)
        {
            int numberOfUnitsInRow = 0;
            int countOfRowaWithThreeUnits = 0;
            foreach (int i in indexes)
            {
                foreach (int j in indexes)
                    if (matrix[i, j] == 1)
                        numberOfUnitsInRow++;

                if (numberOfUnitsInRow == 3)
                    countOfRowaWithThreeUnits++;

                numberOfUnitsInRow = 0;
            }

            if (countOfRowaWithThreeUnits == 6)
                return true;

            return false;
        }

        private bool CheckOnK33AllSubGrphs()
        {
            foreach (int[] indexes in Combinations.Make(6, matrix.GetLength(0)))
                if (CheckOnK5(indexes))
                    return true;

            return false;
        }

        private bool CanConstrict(int[] indexes, int i, int j, List<int> constrictedIndexes)
        {
            if (i == j)
                return false;

            for (int counter = 0; counter < matrix.GetLength(0); counter++)
            {
                if(!indexes.Contains(counter) && !constrictedIndexes.Contains(counter))
                {
                    if (matrix[counter, i] == 1 && matrix[counter, j] == 1)
                    {
                        constrictedIndexes.Add(counter);
                        return true;
                    }
                }
            }

            return false;
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
