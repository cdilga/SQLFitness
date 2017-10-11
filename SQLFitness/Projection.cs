using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Projection : Chromosome
    {
        private readonly List<string> _validFields;

        /// <summary>
        /// Takes a database table in to initialise to one of the valid values
        /// These will be immutable
        /// </summary>
        /// <param name="validFields">List of valid options to choose from</param>
        public Projection(List<string> validFields)
        {
            _validFields = validFields;
            //Unsure about OOP design principles here. I've got an abstract class but have to assign values to this in the constructor of the base classes

            _field = validFields.GetRandomValue();
        }

        //TODO fix this unimplemented thing
        public override Chromosome Mutate() => new Projection(_validFields);
    }
}
