namespace Planner
{
    class Programm
    {
        static void Main(string[] args)
        {
            Database dbase = new Database("D:\\planner.sqlite");
            System.Console.WriteLine("Нажмите любую клавишу для выхода...");
            System.Console.ReadKey();
        }
    }
}
