using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public abstract class Chromosome
    {

        public abstract Chromosome Mutate();
        protected string _field;
        virtual public string Field { get => _field; }
        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => this.Field;

        //A property must exist that is then used by the other classes to generate MySQL queries, perferably 
        //non stringy so that it can be easily swapped out with another language
        //TODO Investigate expression trees as per https://stackoverflow.com/questions/9505189/dynamically-generate-linq-queries

        string ToSql();
    }
}
