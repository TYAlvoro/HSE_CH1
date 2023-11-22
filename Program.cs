namespace Сontrol_Homework
{
    /// <summary>
    /// The class in which the main method of the program is implemented.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main method that binds all methods from the Methods class together.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // A design for getting a separator installed in the system
            // and going to the folder where the program file is located.
            char separator = Path.DirectorySeparatorChar;
            Directory.SetCurrentDirectory($"..{separator}..{separator}..");

            // A cycle in which the possibility of code reuse is implemented. Works until Esc is pressed.
            do
            {
                int matrixOrder;
                string fileName;

                Methods.InputOrder(out matrixOrder);
                Methods.InputFileName(out fileName);

                double[,] matrix = Methods.ReturnArray(matrixOrder);
                Methods.WriteToFile(matrix, fileName);

                Console.WriteLine("Для выхода нажмите Esc. Для повторного ввода нажмите на любую клавишу...");

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}