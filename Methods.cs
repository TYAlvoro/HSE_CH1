using System.Runtime.ConstrainedExecution;

namespace Сontrol_Homework
{
    /// <summary>
    /// This class contains methods for entering the order of the matrix, 
    /// the name of the output file, 
    /// as well as methods for creating and writing a matrix to a file.
    /// </summary>
    internal class Methods
    {
        /// <summary>
        /// This method checks the correctness of entering the order of the matrix and performs this input.
        /// </summary>
        /// <param name="matrixOrder">Matrix order.</param>
        public static void InputOrder(out int matrixOrder)
        {
            Console.WriteLine("Введите порядок матрицы...");

            // A variable that will store the Boolean value of an expression
            // that checks whether the entered integer within the range (0;15].
            bool correctOrder;

            // Directly a loop that runs until the entered value meets the requirements.
            do
            {
                correctOrder = int.TryParse(Console.ReadLine(), out matrixOrder) && matrixOrder > 0 && matrixOrder <= 15;
                if (!correctOrder)
                {
                    Console.WriteLine("Ошибка ввода! Пожалуйста, введите целое число в диапазоне (0, 15]...");
                }

            } while (!correctOrder);
        }

        /// <summary>
        /// This method checks the correctness of the input of the output file name and performs this input.
        /// </summary>
        /// <param name="fileName">Name of file.</param>
        public static void InputFileName(out string fileName)
        {
            Console.WriteLine("Отлично! Теперь введите название выходного файла без разрешения в формате <имя файла>");

            // A variable that will store a Boolean value whether the entered file name is correct.
            bool correctName;

            // Directly a loop that runs until the entered value meets the requirements.
            do
            {
                fileName = Console.ReadLine();
                correctName = true;

                // A loop that checks for the presence of characters prohibited in file names.
                foreach (char symbol in "/\\*:?|\".<>")
                {
                    if (fileName == null || fileName.Contains(symbol))
                    {
                        correctName = false;
                        Console.WriteLine("В имени файла содержится запрещенный символ! Не используйте символы /\\*:?|\"<> и комбинации клавиш!");
                        break;
                    }
                }

            } while (!correctName);
        }

        /// <summary>
        /// A method that builds a matrix of the desired order according to the described rules.
        /// </summary>
        /// <param name="matrixOrder">Matrix order.</param>
        /// <returns>Returns the constructed matrix.</returns>
        public static double[,] ReturnArray(int matrixOrder)
        {
            double[,] matrix = new double[matrixOrder, matrixOrder];

            // The counter that participates in the formula for counting each element.
            int counter = 1;

            // A system of nested loops that run through all the elements
            // of the matrix and fill them according to the rule.
            for (int i = 0; i < matrixOrder; i++)
            {
                for (int j = 0; j < matrixOrder; j++)
                {
                    matrix[i, j] = Math.Round((Math.Pow(2 - counter, 3)) / (Math.Pow(counter + 1, 2) - Math.Pow(counter + 1, 3)), 2);
                    counter++;
                }
            }
            return matrix;
        }

        /// <summary>
        /// A method that writes matrix elements to a file according to the rules specified in the task.
        /// </summary>
        /// <param name="matrix">The matrix to be written to the file.</param>
        /// <param name="fileName">The name of the output file.</param>
        public static void WriteToFile(double[,] matrix, string fileName)
        {
            // A variable that stores the path to the output file.
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.txt");

            // A variable that gets the order of the matrix using the built-in method.
            int matrixOrder = matrix.GetLength(0);

            // A construction that, using a StreamWriter and a nested loop,
            // runs through the entire matrix and writes each element on a new line,
            // if file is ready for write.
            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    for (int i = 0; i < matrixOrder; i++)
                    {
                        for (int j = 0; j < matrixOrder; j++)
                        {
                            outputFile.WriteLine(matrix[i, j].ToString("0.00"));
                        }
                        outputFile.WriteLine();
                    }
                }

                Console.WriteLine($"Информация успешно записана в файл {fileName}!");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Ошибка! Файл недоступен для записи! {e.GetType().Name}");
            }
        }
    }
}
