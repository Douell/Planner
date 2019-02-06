using System.Data.SQLite;

namespace Planner
{
    //Class is created for incapsulating a command itself and the data about it.
    abstract class Command
    {
        protected string Name; //Command name which is used to launch it.
        protected string Description; //Command description. This is used in the help output.
        protected readonly SQLiteConnection Connect;

        protected Command(SQLiteConnection conn, string name, string description)
        {
            Name = name;
            Description = description;
            Connect = new SQLiteConnection(conn);
        }

        public abstract void Commit();

        public string GetName()
        {
            return Name;
        }

        public override string ToString()
        {
            return string.Format("{0, -10}\t{1}", Name, Description);
        }
    }
}
