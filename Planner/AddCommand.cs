using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;

namespace Planner
{
    class AddCommand : Command
    {
        public AddCommand(SQLiteConnection connect, string name, string description) : base(connect, name, description) { }

        public override void Commit()
        {
            string name = "";
            string descr = "";
            string date = "";
            string time = "";
            string priority = "";
            //Inserting task name.
            do
            {
                Console.Write("{0}> Введите название мероприятия: ", this.Name);
            } while ((name = Console.ReadLine()) == "");
            //Inserting task description.
            do
            {
                Console.Write("{0}> Введите описание мероприятия: ", this.Name);
            } while ((descr = Console.ReadLine()) == "");
            //Inserting task date.
            while (true)
            {
                Console.Write("{0}> Введите дату проведения мероприятия в формате ДД.ММ.ГГГГ: ", this.Name);
                date = Console.ReadLine();
                if (date == "")
                {
                    //Setting up the default value.
                    date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                    break;
                }
                else if (CheckDate(date))
                {
                    date = FormatDate(date);
                    break;
                }
                else
                {
                    Console.WriteLine("Дата не соответствует заданному формату");
                }

            }
            //Inserting task time.
            while (true)
            {
                Console.Write("{0}> Введите время проведения мероприятия в формате ЧЧ:ММ (24ч): ", this.Name);
                time = Console.ReadLine();
                if (time == "")
                {
                    //Setting up the default value.
                    time = DateTime.Now.ToString("HH:mm");
                    break;
                }
                else if (CheckTime(time))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Время не соответствует заданному формату");
                }

            }
            //Inserting task priority.
            while (true)
            {
                Console.Write("{0}> Введите приоритет задачи (1 - высокий, 2 - средний, 3 - низкий): ", this.Name);
                priority = Console.ReadLine();
                if (priority == "")
                {
                    //Setting up the default value.
                    priority = "2";
                    break;
                }
                //Searching inserted priority value in available ones.
                else if (Array.Exists(new string[3] { "1", "2", "3" }, prSearch => prSearch == priority))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Введённое значение приоритета не подходит. Повторите ввод");
                }
            }
            //Creating SQL commands.
            SQLiteCommand foreignKey = new SQLiteCommand("PRAGMA foreign_keys=ON", this.Connect);
            string[] stringParams = { "@name", "@desc", "@date", "@time" };
            SQLiteCommand add = new SQLiteCommand(
                String.Format("INSERT INTO Tasks(name, description, date, time, priority) " +
                "VALUES ({0}, @priority)", String.Join(", ", stringParams)), this.Connect);
            //Creating commands parameters.
            foreach (var param in stringParams)
            {
                add.Parameters.Add(param, DbType.String);
            }
            add.Parameters.Add("@priority", DbType.UInt32);
            string[] stringValues = { name, descr, date, time };
            foreach (var i in stringParams.Zip(stringValues, (pr, vl) => new { pr, vl }))
            {
                add.Parameters[i.pr].Value = i.vl;
            }
            add.Parameters["@priority"].Value = Convert.ToInt32(priority);
            Connect.Open();
            foreignKey.ExecuteNonQuery();
            add.ExecuteNonQuery();
            Connect.Close();
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
