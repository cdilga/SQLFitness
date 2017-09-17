using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace SQLFitness
{
    public class Projection : IChromosome
    {
        public string Field { get; }
        private readonly List<string> _validFields;

        /// <summary>
        /// Takes a database table in to initialise to one of the valid values
        /// These will be immutable
        /// </summary>
        /// <param name="validFields">List of valid options to choose from</param>
        public Projection(List<string> validFields)
        {
            _validFields = validFields;
            var index = Utility.GetRandomNum(validFields.Count);
            this.Field = validFields[index];
        }

        public IChromosome Mutate() => new Projection(_validFields);



        //    //Has a value for select
        //    //Get a database table - pull all possible values in (columns)
        //    //Initialise the projection to a random one of these 

        //    //Get out the first row
        //    var command = new MySqlCommand("SELECT * FROM country WHERE 1 = 0", conn);
        //    conn.Open();

        //        //Iterate through all of the rows and pick a row number and a type
        //        IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
        //    DataTable schema = reader.GetSchemaTable();
        //        for (var i = 0; i<reader.FieldCount; i++)
        //        {
        //            Console.Write(schema.ToString());
        //        }

        //conn.Close();
    }
}
