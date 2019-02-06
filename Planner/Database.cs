using System.IO;
using System.Data.SQLite;
using System.Data;

namespace Planner
{
    class Database
    {
        //Connection to database with tasks.
        public SQLiteConnection Connection;

        public Database(string DBName)
        {
            //Creating the empty .sqlite file with necessary tables if it does not exist.
            if (File.Exists(DBName))
            {
                this.Connection = new SQLiteConnection(string.Format("Data Source = {0}", DBName));
            }
            else
            {
                //Creating .sqlite file.
                SQLiteConnection.CreateFile(DBName);
                try
                {
                    //Creating tables in database.
                    this.Connection = new SQLiteConnection(string.Format("Data Source = {0}", DBName));
                    SQLiteCommand priorTable = new SQLiteCommand(
                        "CREATE TABLE Priority (id INTEGER PRIMARY KEY, name text(255))", Connection);
                    SQLiteCommand priors = new SQLiteCommand(
                        "INSERT INTO Priority(name) VALUES(@prior_name)", Connection);
                    priors.Parameters.Add("@prior_name", DbType.String);
                    SQLiteCommand tasksTable = new SQLiteCommand(
                        "CREATE TABLE Tasks(" +
                        "id INTEGER PRIMARY KEY," +
                        "name text(255)," +
                        "description text(255)," +
                        "date text," +
                        "time text," +
                        "priority int REFERENCES Priority(id))", Connection);
                    Connection.Open();
                    priorTable.ExecuteNonQuery();
                    foreach (var prName in new string[3] { "Высокий", "Средний", "Низкий" })
                    {
                        priors.Parameters["@prior_name"].Value = prName;
                        priors.ExecuteNonQuery();
                    }
                    tasksTable.ExecuteNonQuery();
                    Connection.Close();
                }
                catch (System.Exception)
                {
                    throw;
                }
                System.Console.WriteLine("База данных создана... {0}", DBName);
            }

        }
    }
}
