using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace SQLFitness
{
    public class Projection : IChromosome
    {
        private string _column { get; }


        /// <summary>
        /// Takes a database table in to initialise to one of the valid values
        /// </summary>
        public Projection(MySqlConnection conn)
        {
            //Has a value for select
            //Get a database table - pull all possible values in (columns)
            //Initialise the projection to a random one of these 

            //Get out the first row
            var command = new MySqlCommand("SELECT * FROM country WHERE 1 = 0", conn);
            conn.Open();

            //Iterate through all of the rows and pick a row number and a type
            IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
            DataTable schema = reader.GetSchemaTable();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(schema.ToString());
            }

            conn.Close();
        }

        public void Mutate()
        {

        }
    }
}
