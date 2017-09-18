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

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => this.Field;
    }
}
