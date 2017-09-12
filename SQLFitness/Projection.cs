using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace SQLFitness
{
    public class Projection : IChromosome
    {
        private string _column { get; set; }
        private MySqlConnection _conn { get; }

        /// <summary>
        /// Takes a database table in to initialise to one of the valid values
        /// This is currently not able to deal with DBMS other than MySql as it's types are 
        /// dependent on MySql.Data.MySqlClient
        /// </summary>
        public Projection(MySqlConnection conn)
        {
            this._conn = conn;
            Mutate(conn);
        }

        public void Mutate()
        {
            Mutate(this._conn);
        }

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

        public string ToSql() => this._column;

    }
}
