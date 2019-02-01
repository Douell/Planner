using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;
using System;

namespace Planner
{
    class Database
    {
        //Database name.
        private string DatabaseName;

        public Database(string db_name)
        {
            DatabaseName = db_name;
            if (!(this.IsExists()))
            {
                this.Create(this.DatabaseName);
                Console.WriteLine("База данных создана");
            }
        }

        bool IsExists()
        {
            return File.Exists(DatabaseName);
        }

        //Creates database with tasks shedule.
        void Create(string db_name)
        {
            DatabaseName = db_name;
            //Just creating the empty .sqlite file.
            SQLiteConnection.CreateFile(db_name);
            string connString = string.Format("Data Source = {0}", db_name);
            //Command strings for creating tables.
            Queue<string> createStrings = new Queue<string>(2);
            createStrings.Enqueue("CREATE TABLE Priority (id INTEGER PRIMARY KEY, name text(255))");
            createStrings.Enqueue("CREATE TABLE Tasks (" +
                "id integer primary key," +
                "name text(255)," +
                "description text(255)," +
                "date text default(date('now'))," +
                "time text default(time('now'))," +
                "priority int references Priority(id)" +
                ");"
                );
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                try
                {
                    connection.Open();
                    foreach (string i in createStrings)
                    {
                        SQLiteCommand currentCommnand = new SQLiteCommand(i, connection);
                        currentCommnand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
