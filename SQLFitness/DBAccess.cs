using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;


namespace SQLFitness
{
    public class DBAccess
    {
        private readonly string _tableName;
        public string TableName { get => _tableName; }
        const string connStr = Utility.ConnString;
        public MySqlConnection Conn { get; }

        public DBAccess(string tableName)
        {
            _tableName = tableName;
            Console.WriteLine("Connecting");
            this.Conn = new MySqlConnection(connStr);
            Conn.Open();
        }

        public DBAccess() : this(Utility.TableName) { }

        //This could also be implemented via IValidColumns or something similar
        public List<object> ValidDataGetter(string column)
        {
            var dataSet = new List<object>();
            try
            {
                
                var sql = $"SELECT {column} FROM {_tableName}";
                var cmd = new MySqlCommand(sql, Conn);
                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    dataSet.Add(reader[0]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
            return dataSet;
        }
        public void Close()
        {
            Conn.Close();
        }
        public List<string> ValidColumnGetter()
        {
            

            //Has a value for select
            //Get a database table - pull all possible values in (columns)
            //Initialise the projection to a random one of these 

            //Get out the first row
            var columnList = new List<string>();
            try
            {
                Conn.Open();

                var command = new MySqlCommand($"SELECT * FROM {_tableName} WHERE 1 = 0" , Conn);
                //Iterate through all of the rows and pick a row number and a type
                var reader = command.ExecuteReader();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    columnList.Add(reader.GetName(i));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Conn.Close();
            return columnList;

        }
    }
}
