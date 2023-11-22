namespace Changer
{
    /// <summary>
    /// The class of the program containing the main method.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// A method that brings together all the implemented methods in the Method class.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // A design for getting a separator installed in the system
            // and going to the folder where the .txt must be located.
            char sep = Path.DirectorySeparatorChar;
            Directory.SetCurrentDirectory($"..{sep}..{sep}..{sep}..{sep}Сontrol Homework");

            // A loop that will allow the program to run repeatedly until Esc is pressed.
            do
            {
                string filePath;
                Methods.ReturnFilePath(out filePath);

                // If the file exists, then you can work with it.
                if (File.Exists(filePath))
                {
                    int matrixOrder = 0;

                    // If the file is of the correct format, then we build the matrix B.
                    if (Methods.RightFormat(filePath, ref matrixOrder))
                    {
                        double[,] originalMatrix = Methods.Reader(filePath, matrixOrder);
                        double[,] finalMatrix = Methods.CreateMatrixB(originalMatrix, matrixOrder);
                        Methods.SwapStrings(finalMatrix, matrixOrder);
                        Methods.PrintResult(originalMatrix, finalMatrix, matrixOrder);
                    }
                }
                else
                {
                    Console.WriteLine($"Файл {filePath} не существует или не может быть открыт.");
                }

                Console.WriteLine("Для выхода нажмите Esc. Для повторного ввода нажмите на любую клавишу...");

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}