using System;
using System.Data.SQLite;

namespace Planner
{
    class Programm
    {
        //TODO: needed commands: create, add, delete, update, show.
        const string HELP = "помощь";
        const string EXIT = "выход";

        public static void Main(string[] arg)
        {
            Database dbase = new Database(".\\planner.sqlite");
            Controller controller = new Controller(dbase);
            Hello();
            string command = "";
            while (true)
            {
                Console.Write("Планировщик>");
                command = Console.ReadLine().Trim();
                if (string.Equals(command, ""))
                {
                    continue;
                }
                if (string.Equals(command, EXIT))
                {
                    break;
                }
                else if (string.Equals(command, HELP))
                {
                    Help(controller);
                    continue;
                }
                Command inputCommand = controller.FindCommand(command);
                if (inputCommand != null)
                {
                    inputCommand.Commit();
                }
                else
                {
                    Console.WriteLine("Команда не распознана");
                }
            }
        }

        private static void Help(Controller commands)
        {
            Console.WriteLine("Доступные команды:");
            foreach (var command in commands)
            {
                Console.WriteLine(command);
            }
            Console.WriteLine("{0, -10}\tВызов справки", HELP);
            Console.WriteLine("{0, -10}\tВыход", EXIT);
        }

        private static void Hello()
        {
            Console.WriteLine("Планировщик v0.0.1. Текущие дата/время: {0}", DateTime.Now.ToString("ddd, dd/MM/yyyy H:mm"));
            Console.WriteLine("Для вызова помощи наберите \"{0}\"", HELP);
            Console.WriteLine("----------");
            Console.WriteLine("На сегодня у Вас были запланированы следующие мероприятия:");
            Console.WriteLine();
        }
    }
}
