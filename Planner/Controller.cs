using System.Collections;
using System.Collections.Generic;

namespace Planner
{
    //Class created for keeping available commands.
    class Controller : IEnumerable<Command>
    {
        private List<Command> Commands;

        public Controller(Database dB)
        {
            Commands = new List<Command>
            {
                new AddCommand(dB.Connection, "добавить", "Добавить мероприятие в список"),
                new ShowCommand(dB.Connection, "показать", "Вывести список всех запланированных мероприятий")
            };
        }

        public Command FindCommand(string name)
        {
            return Commands.Find(comm => string.Equals(comm.GetName(), name));
        }

        public IEnumerator<Command> GetEnumerator()
        {
            foreach (var command in Commands)
            {
                yield return command;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
