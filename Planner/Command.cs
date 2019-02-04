using System.Data.SQLite;

namespace Planner
{
    //Class is created for incapsulate data about command and SQL command itself.
    abstract class Command
    {
        public readonly string Name;
        public readonly string Description;
        private readonly SQLiteCommand Cmd;

        protected Command(SQLiteConnection connect, string name, string description)
        {
            Name = name;
            Description = description;
            Cmd = new SQLiteCommand(connect);
        }
    }
}
