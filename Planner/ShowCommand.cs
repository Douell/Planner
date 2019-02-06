using System;
using System.Data.SQLite;

namespace Planner
{
    class ShowCommand : Command
    {
        public ShowCommand(SQLiteConnection connect, string name, string description) : base(connect, name, description) { }

        public override void Commit()
        {
            SQLiteCommand show = new SQLiteCommand(
                "SELECT tasks.id, tasks.name, tasks.description, strftime(\"%d.%m.%Y\", tasks.date), tasks.time, priority.name FROM tasks " +
                "JOIN priority " +
                "WHERE tasks.priority = priority.id", Connect);
            Connect.Open();
            SQLiteDataReader rez = show.ExecuteReader();

            if (rez.HasRows)
            {
                while (rez.Read())
                {
                    Console.Write("{0}.\tНазвание:{1}\n" +
                        "\tОписание: {2}\n" +
                        "\tДата/время: {3}, {4}\n" +
                        "\tПриоритет: {5}\n", 
                        rez.GetInt32(0), rez.GetString(1), rez.GetString(2), rez.GetString(3), rez.GetString(4), rez.GetString(5));
                }
            }
            Connect.Close();
        }
    }
}
