using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Selection: Projection
    {
        private enum Operator { equal, notEqual, greaterThan, lessThan, greaterThanEqual, lessThanEqual }
        private string _operator { get; set; }
        private string _condition { get; set; }
        private string _column { get; set; }

        public Selection ()
        {
            //Has an operator
            //Has a condition (might be a numerical value or something) - sourced from the database
            //Has a value (i.e. attribute name)
        }
        
        public Mutate()
        {
            public void Mutate(MySqlConnection conn)
            {
                //Has a value for select
                //Get a database table - pull all possible values in (columns)
                //Initialise the projection to a random one of these 

                try
                {
                    Console.WriteLine("Connecting");
                    conn.Open();
                    var sql = "SELECT * FROM country";
                    var cmd = new MySqlCommand(sql, conn);
                    var reader = cmd.ExecuteReader();

                    //Get a random column name
                    var random = new Random();
                    _column = reader.GetName(random.Next(0, reader.FieldCount));

                    //var testProjection = new Projection(reader);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                conn.Close();
                Console.WriteLine("Done.");

            }

        }
    }
}
