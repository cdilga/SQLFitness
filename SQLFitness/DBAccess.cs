using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SQLFitness
{
    public class DBAccess
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
            var dataSet = new List<object>();
            try
            {
                Console.WriteLine("Connecting");
                conn.Open();
                var sql = $"SELECT {column} FROM {_tableName}";
                var cmd = new MySqlCommand(sql, conn);
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

            conn.Close();
            return dataSet;
        }

        public double evaluateFitness(string sql)
        {
            var fieldDist = 0;
            var rowDist = 0;
            var numRowDist = 0;
            try
            {
                Console.WriteLine("Connecting");
                conn.Open();
                var cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                var dataList = new List<List<object>>();

                numRowDist = Math.Abs(reader.FieldCount - 1);
                //Get the column name of the first column (the only one that is going to matter here)
                reader.Read();
                //Do a new query for the schema, and find the distance to the name
                fieldDist = ValidColumnGetter().IndexOf(reader[0].ToString());
                do
                {
                    //TODO find where else this is to prevent having huge rows without resetting
                    var rowList = new List<object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        rowList.Add(reader[i]);
                    }
                    dataList.Add(rowList);
                } while (reader.Read());
                reader.Close();

                //Couldn't be bothered writing this for specific rows - too hard
                rowDist = Math.Abs(10 - dataList.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return fieldDist + rowDist + numRowDist;
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
                conn.Open();

                var command = new MySqlCommand($"SELECT * FROM {_tableName} WHERE 1 = 0" , conn);
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
            conn.Close();
            return columnList;

        }
    }
}
