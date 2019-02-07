using System;
using System.Data;
using System.Data.SQLite;

namespace Planner
{
    class DeleteCommand : Command
    {
        public DeleteCommand(SQLiteConnection connect, string name, string description) : base(connect, name, description) { }

        public override void Commit()
        {
            //Read all task ID's from database.
            string readCommand = "SELECT id FROM Tasks";
            SQLiteDataAdapter readTaskID = new SQLiteDataAdapter(readCommand, Connect);
            DataSet ids = new DataSet();
            readTaskID.Fill(ids, "id");
            //Creating command for deleting necessary task.
            SQLiteCommand comm = new SQLiteCommand("DELETE FROM Tasks WHERE id = @id", Connect);
            comm.Parameters.Add("@id", DbType.Int32);
            //Inserting ID of necessary task.
            string idString = "";
            do
            {
                Console.Write("{0}> Введите номер мероприятия: ", Name);
            } while ((idString = Console.ReadLine()) == "");
            int id;
            try
            {
            id = Convert.ToInt32(idString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Введённое значение не является числом");
                return;
            }
            //Checking that inserted ID is present in database.
            int idAmount = ids.Tables["id"].Select(string.Format("id = {0}", id)).Length;
            if (idAmount != 0)
            {
                //Deleting a task.
                comm.Parameters["@id"].Value = id;
                Connect.Open();
                comm.ExecuteNonQuery();
                Connect.Close();
                Console.WriteLine("Мероприятие {0} удалено", id);
            }
            else
            {
                Console.WriteLine("Заданное мероприятие не найдено");
            }
        }
    }
}
