using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SQLFitness
{
    class DBAccess
    {
        private readonly string _tableName;
        const string connStr = Utility.ConnString;
        private MySqlConnection conn = new MySqlConnection(connStr);

        public DBAccess(string tableName)
        {
            _tableName = tableName;

        }
            

        public DBAccess() : this(Utility.TableName)
        {
        }

        //This could also be implemented via IValidColumns or something similar
        public List<object> ValidDataGetter(string column)
        {
            var dataList = new List<object>();
            try
            {
                Console.WriteLine("Connecting");
                conn.Open();
                var sql = $"SELECT {column} FROM {_tableName}";
                var cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                
                foreach (var dataItem in reader)
                {
                    dataList.Add(dataItem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            return dataList;
        }

        //something that executes sql and returns something that can be evaluated by some fitness function

        public List<string> ValidColumnGetter()
        {
            

            //Has a value for select
            //Get a database table - pull all possible values in (columns)
            //Initialise the projection to a random one of these 

            //Get out the first row
            var columnList = new List<string>();
            try
            {
                conn.Open();

                var command = new MySqlCommand($"SELECT * FROM {_tableName}" , conn);
                //Iterate through all of the rows and pick a row number and a type
                IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
                DataTable schema = reader.GetSchemaTable();
                
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    columnList.Add(schema.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return columnList;
        }
    }
}
