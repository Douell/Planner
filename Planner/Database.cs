using System.IO;
using System.Data.SQLite;

namespace Planner
{
    class Database
    {
        //Connection to database with tasks.
        public SQLiteConnection DatabaseConnection;

        public Database(string DBName)
        {
            //Creating the empty .sqlite file with necessary tables if it does not exist.
            if (!File.Exists(DBName))
            {
                //Creating .sqlite file.
                SQLiteConnection.CreateFile(DBName);
                try
                {
                    //Creating tables in database.
                    SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source = {0}", DBName));
                    SQLiteCommand foreignKey = new SQLiteCommand("PRAGMA foreign_keys=ON", connection);
                    SQLiteCommand prior = new SQLiteCommand("CREATE TABLE Priority (id INTEGER PRIMARY KEY, name text(255))", connection);
                    SQLiteCommand tasks = new SQLiteCommand(
                        "CREATE TABLE Tasks(" +
                        "id INTEGER PRIMARY KEY," +
                        "name text(255)," +
                        "description text(255)," +
                        "date text default(date('now'))," +
                        "time text default(time('now'))," +
                        "priority int REFERENCES Priority(id))", connection);
                    connection.Open();
                    foreignKey.ExecuteNonQuery();
                    prior.ExecuteNonQuery();
                    tasks.ExecuteNonQuery();
                    connection.Close();
                }
                catch (System.Exception)
                {
                    throw;
                }
                System.Console.WriteLine("База данных создана... {0}", DBName);
            }
            try
            {
                DatabaseConnection = new SQLiteConnection(DBName);
            }
            catch (System.Exception)
            {
                throw;
            }

        }
    }
}
