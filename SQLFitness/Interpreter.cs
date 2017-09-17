using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SQLFitness
{
    class Interpreter
    {
        private readonly Individual _sqlIndividual;
        private readonly string _tableName;

        //This object is required to communicate with the DBAccess to get a subset of the data back
        //This object will know about SQL Syntax. An interpreter is essentially a kind of parser, so It'll be passed in an individual which it will then convert into something usable
        public Interpreter(Individual sqlIndividual, string tableName)
        {
            _sqlIndividual = sqlIndividual;
            _tableName = tableName;
        }

        private string _generateSql(Projection chromosome)
        {
            
        }

        private string _generateSql(List<IChromosome> chromosomes)
        {
            foreach (IChromosome chromosome in chromosomes)
            {
                //basically, if it's projection we are going to add it in after SELECT and before anything else FROM tableName
                //or if it's a projection we're going to have to 
            }
            List<IChromosome> _projections = chromosomes.Where(chromosome => chromosome.GetType() == typeof(Projection)).ToList<IChromosome>();

            var catenatedProjections = String.Join(" ", _projections);
            return $"SELECT { catenatedProjections } FROM { _tableName }";
        }
    }
}
