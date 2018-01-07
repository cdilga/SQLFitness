using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class DBSelector : IFitness
    {
        /// <summary>
        /// A DBSelector is what allows selection to be done on an individual, so a selection implements a fitness function
        /// </summary>
        private DBAccess _db;
        private Interpreter _interpreter;
        const string connStr = Utility.ConnString;
        //private MySqlConnection conn = new MySqlConnection(connStr);

        public DBSelector(DBAccess db, Interpreter interpreter)
        {
            _interpreter = interpreter;
            _db = db;
        }

        public double[] Evaluate(StubIndividual individual)
        {
            Console.WriteLine(_interpreter.Parse(individual));
            double[] output = new double[2];
            output[0] = 1.0;
            return output;
            var fieldDist = 0;
            var rowDist = 0;
            var numRowDist = 0;
            try
            {
                _db.Conn.Open();
                //Console.WriteLine("Connecting");
                var cmd = new MySqlCommand(_interpreter.Parse(individual), _db.Conn);
                Console.WriteLine(cmd.CommandText);
                var reader = cmd.ExecuteReader();
                var dataList = new List<List<object>>();

                numRowDist = Math.Abs(reader.FieldCount - 1);
                //Get the column name of the first column (the only one that is going to matter here)
                reader.Read();
                //Do a new query for the schema, and find the distance to the name
                fieldDist = _db.ValidColumnGetter().IndexOf(reader[0].ToString());
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
            finally
            {
                //TODO fix this 
                _db.Conn.Close();
            }
            output[0] = fieldDist + rowDist + numRowDist;
            return output;
        }
    }
}
