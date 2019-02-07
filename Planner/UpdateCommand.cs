using System;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Planner
{
    class UpdateCommand : Command
    {
        public UpdateCommand(SQLiteConnection conn, string name, string description) : base(conn, name, description) { }

        public override void Commit()
        {
            //Read all task ID's from database.
            string readCommand = "SELECT id FROM Tasks";
            SQLiteDataAdapter readTaskID = new SQLiteDataAdapter(readCommand, Connect);
            DataSet ids = new DataSet();
            readTaskID.Fill(ids, "id");
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
                string paramNum = "";
                //A task parameter value which is being updated.
                string value = "";
                do
                {
                    Console.WriteLine("{0}> Выберите номер редактируемого параметра: ", Name);
                    Console.WriteLine("\t[1]Имя\n\t[2]Описание\n\t[3]Дата\n\t[4]Время\n\t[5]Приоритет");
                    Console.Write("{0}> ", Name);
                } while ((paramNum = Console.ReadLine()) == "" || Convert.ToInt32(paramNum) < 1 || Convert.ToInt32(paramNum) > 5);
                SQLiteCommand update;
                switch (Convert.ToInt32(paramNum))
                {
                    case (1):
                        do
                        {
                            Console.Write("{0}> Введите новое имя: ", Name);
                            value = Console.ReadLine();
                        } while (value == "");
                        update = new SQLiteCommand(
                            "UPDATE TASKS " +
                            "SET name = @value " +
                            "WHERE id = @id", Connect);
                        update.Parameters.Add("@value", DbType.String);
                        update.Parameters.Add("@id", DbType.Int32);
                        update.Parameters["@value"].Value = value;
                        update.Parameters["@id"].Value = id;
                        Connect.Open();
                        update.ExecuteNonQuery();
                        Connect.Close();
                        break;
                    case (2):
                        do
                        {
                            Console.Write("{0}> Введите новое описание: ", Name);
                            value = Console.ReadLine();
                        } while (value == "");
                        update = new SQLiteCommand(
                         "UPDATE TASKS " +
                         "SET description = @value " +
                         "WHERE id = @id", Connect);
                        update.Parameters.Add("@value", DbType.String);
                        update.Parameters.Add("@id", DbType.Int32);
                        update.Parameters["@value"].Value = value;
                        update.Parameters["@id"].Value = id;
                        Connect.Open();
                        update.ExecuteNonQuery();
                        Connect.Close();
                        break;
                    case (3):
                        while (true)
                        {
                            Console.Write("{0}> Введите новую дату в формате ДД.ММ.ГГГГ: ", Name);
                            value = Console.ReadLine();
                            if (CheckDate(value))
                            {
                                value = FormatDate(value);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Дата не соответствует заданному формату");
                            }
                        }
                        update = new SQLiteCommand(
                         "UPDATE TASKS " +
                         "SET date = @value " +
                         "WHERE id = @id", Connect);
                        update.Parameters.Add("@value", DbType.String);
                        update.Parameters.Add("@id", DbType.Int32);
                        update.Parameters["@value"].Value = value;
                        update.Parameters["@id"].Value = id;
                        Connect.Open();
                        update.ExecuteNonQuery();
                        Connect.Close();
                        break;
                    case (4):
                        while (true)
                        {
                            Console.Write("{0}> Введите новое время в формате ЧЧ:ММ (24ч): ", Name);
                            value = Console.ReadLine();
                            if (CheckTime(value))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Время не соответствует заданному формату");
                            }
                        }
                        update = new SQLiteCommand(
                       "UPDATE TASKS " +
                       "SET time = @value " +
                       "WHERE id = @id", Connect);
                        update.Parameters.Add("@value", DbType.String);
                        update.Parameters.Add("@id", DbType.Int32);
                        update.Parameters["@value"].Value = value;
                        update.Parameters["@id"].Value = id;
                        Connect.Open();
                        update.ExecuteNonQuery();
                        Connect.Close();
                        break;
                    case (5):
                        do
                        {
                            Console.Write("{0}> Введите новый приоритет (1 - высокий, 2 - средний, 3 - низкий): ", Name);
                            value = Console.ReadLine();
                        }
                        while (!(Array.Exists(new string[3] { "1", "2", "3" }, prSearch => prSearch == value)));
                        SQLiteCommand updatePrior = new SQLiteCommand(
                            "UPDATE TASKS " +
                            "SET priority = @value " +
                            "WHERE id = @id", Connect);
                        updatePrior.Parameters.Add("@value", DbType.Int32);
                        updatePrior.Parameters.Add("@id", DbType.Int32);
                        updatePrior.Parameters["@value"].Value = Convert.ToInt32(value);
                        updatePrior.Parameters["@id"].Value = id;
                        Connect.Open();
                        updatePrior.ExecuteNonQuery();
                        Connect.Close();
                        return;
                    default:
                        return;
                }
            }
            else
            {
                Console.WriteLine("Заданное мероприятие не найдено");
            }
        }

        private bool CheckDate(string dat)
        {
            string dateRegex = "([0-2][0-9]|30|31)\\.(0[1-9]|10|11|12)\\.(\\d{4})";
            return Regex.IsMatch(dat, dateRegex);
        }

        private string FormatDate(string dat)
        {
            return Regex.Replace(dat, "([0-2][0-9]|30|31)\\.(0[1-9]|10|11|12)\\.(\\d{4})", "$3-$2-$1");
        }

        private bool CheckTime(string tim)
        {
            string timeRegex = "([0-1][0-9]|2[0-3]):([0-5][0-9])";
            return Regex.IsMatch(tim, timeRegex);
        }
    }
}
