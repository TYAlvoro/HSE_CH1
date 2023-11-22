using System.Text.RegularExpressions;

namespace Changer
{
    /// <summary>
    /// A class that implements all the methods required by the task.
    /// </summary>
    internal class Methods
    {
        /// <summary>
        /// A method that forms the path to the input file and saves it.
        /// </summary>
        /// <param name="filePath">The variable in which the path will be written.</param>
        public static void ReturnFilePath(out string filePath)
        {
            Console.WriteLine("Введите название входного файла в формате <имя файла> без расширения...");

            string fileName = $"{Console.ReadLine()}.txt";
            filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        }

        /// <summary>
        /// A method that performs a full check for the correct format of the data in the file.
        /// </summary>
        /// <param name="filePath">The path to the input file.</param>
        /// <param name="matrixOrder">The order of the matrix.</param>
        /// <returns>Returns whether the format is correct.</returns>
        public static bool RightFormat(string filePath, ref int matrixOrder)
        {
            // A variable for counting the number of numbers up to an empty line.
            int numCounter = 0;
            // A variable for counting the number of empty lines in a file.
            int emptyLineCounter = 0;

            // A construction that allows you to bypass the entire file and check its format.
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;

                // This loop runs until it reaches the last line, which is null, that is, the end of the file.
                while ((line = reader.ReadLine()) != null)
                {
                    // Checking options if an empty string is encountered.
                    if (line.Length == 0)
                    {
                        // Not a single number was found and the order is also 0,
                        // hence an empty line at the beginning of the file, the format is broken.
                        if (matrixOrder == 0 && numCounter == 0)
                        {
                            Console.WriteLine("Пустой файл или пустая строка в начале файла");
                            return false;
                        }

                        // The numbers found, the order is still zero. So the first row of the matrix has ended.
                        // We write the number of numbers as an order.
                        else if (matrixOrder == 0 && numCounter != 0)
                        {
                            matrixOrder = numCounter;
                            numCounter = 0;
                            emptyLineCounter++;
                        }

                        // If the order is already defined and the numbers are found.
                        else if (matrixOrder != 0 && numCounter != 0)
                        {
                            // If the number of numbers found is equal to the order, then everything is OK, we continue.
                            if (numCounter == matrixOrder)
                            {
                                numCounter = 0;
                                emptyLineCounter++;
                            }

                            // Otherwise, the format is broken.
                            else
                            {
                                Console.WriteLine("Неверный формат данных!");
                                return false;
                            }
                        }

                        // The number of empty rows between columns is > 1
                        // or there are too many of them at the end of the file. The format is broken.
                        else
                        {
                            Console.WriteLine("Пустых строк между блоками в конце файла больше, чем нужно");
                            return false;
                        }
                    }

                    // The string is not empty, you can work with its value.
                    else
                    {
                        // If the line has the format "-X.XX" or "X.XX", then continue checking.
                        if ((line[0] == '-' && Regex.IsMatch(line, @"^-?\w\.\w\w$")) || (line[0] != '-' && Regex.IsMatch(line, @"^\w\.\w\w$")))
                        {
                            double num;

                            // If the string is of type double, then it fits exactly.
                            if (double.TryParse(line, out num))
                            {
                                numCounter++;
                            }

                            else
                            {
                                Console.WriteLine("Неверный формат данных!");
                                return false;
                            }
                        }

                        else
                        {
                            Console.WriteLine("Неверный формат данных!");
                            return false;
                        }
                    }
                }

                // If the first line was null, then the file is completely empty.
                if (matrixOrder == 0)
                {
                    Console.WriteLine("Пустой файл");
                    return false;
                }
            }

            // The format is correct if the number of empty rows between columns is equal to the order of the matrix.
            if (emptyLineCounter == matrixOrder)
            {
                return true;
            }

            else
            {
                Console.WriteLine("Неверный формат данных!");
                return false;
            }

        }

        /// <summary>
        /// A method that completely reads a matrix from a file.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <param name="matrixOrder">Order of matrix.</param>
        /// <returns>Returns original matrix without changes.</returns>
        public static double[,] Reader(string filePath, int matrixOrder)
        {
            double[,] originalMatrix = new double[matrixOrder, matrixOrder];
            int row = 0;
            int column = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                // The cycle runs the number of times calculated by a formula based on the order of the matrix.
                for (int i = 0; i < matrixOrder * (matrixOrder + 1); i++)
                {
                    double num;

                    if (double.TryParse(reader.ReadLine(), out num))
                    {
                        originalMatrix[row, column] = num;

                        // As long as the column number does not exceed the number of columns, we do not change the row.
                        if (column != matrixOrder - 1)
                        {
                            column++;
                        }
                        else
                        {
                            column = 0;
                            row++;
                        }

                    }
                }
            }
            return originalMatrix;
        }

        /// <summary>
        /// A method that builds a matrix B initially identical to the original matrix.
        /// </summary>
        /// <param name="originalMatrix">Original matrix.</param>
        /// <param name="matrixOrder">Order of original matrix.</param>
        /// <returns>Return matrix B</returns>
        public static double[,] CreateMatrixB(double[,] originalMatrix, int matrixOrder)
        {
            double[,] finalMatrix = new double[matrixOrder, matrixOrder];

            // A cycle that, for each element of the original matrix, fills the corresponding element of the matrix B.
            for (int i = 0; i < matrixOrder; i++)
            {
                for (int j = 0; j < matrixOrder; j++)
                {
                    finalMatrix[i, j] = originalMatrix[i, j];
                }
            }

            return finalMatrix;
        }

        /// <summary>
        /// A method that swaps the elements of rows symmetrical with respect to the center.
        /// </summary>
        /// <param name="finalMatrix">Matrix B.</param>
        /// <param name="matrixOrder">Order of matrix B.</param>
        public static void SwapStrings(double[,] finalMatrix, int matrixOrder)
        {
            // The number of the last one in front of the center of the row.
            int lastCheckingRow = matrixOrder / 2;

            // The outer loop runs through all the rows to the central one,
            // and the nested one runs through all the elements of each of the rows.
            for (int i = 0; i < lastCheckingRow; i++)
            {
                // The number of the row to swap elements with.
                int rowForSwap = matrixOrder - (i + 1);

                for (int j = 0; j < matrixOrder; j++)
                {
                    // Swap places only if the elements are not equal.
                    if (finalMatrix[i, j] != finalMatrix[rowForSwap, j])
                    {
                        double temp = finalMatrix[i, j];
                        finalMatrix[i, j] = finalMatrix[rowForSwap, j];
                        finalMatrix[rowForSwap, j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// The method that prints the result of the program.
        /// </summary>
        /// <param name="originalMatrix">Original matrix.</param>
        /// <param name="finalMatrix">Matrix B.</param>
        /// <param name="matrixOrder">The order of these matrices.</param>
        public static void PrintResult(double[,] originalMatrix, double[,] finalMatrix, int matrixOrder)
        {
            // The original matrix is displayed line by line on the screen.
            for (int i = 0; i < matrixOrder; i++)
            {
                for (int j = 0; j < matrixOrder; j++)
                {
                    Console.Write(originalMatrix[i, j].ToString("0.00") + " ");
                }

                Console.WriteLine();
            }

            // Skipping a line.
            Console.WriteLine();

            // The matrix B is displayed line by line on the screen .
            for (int i = 0; i < matrixOrder; i++)
            {
                for (int j = 0; j < matrixOrder; j++)
                {
                    Console.Write(finalMatrix[i, j].ToString("0.00") + " ");
                }

                Console.WriteLine();
            }
        }
    }
}