using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SQLFitness
{
    public abstract class StubIndividual
    {
        
        public Fitness Fitness { get; set; }
        //protected List<String> _validColumns;
        //protected Func<string, List<object>> _validDataGetter;

        static StubIndividual()
        {
            _initialiseChromosomeFactory();
        }

        protected static void _initialiseChromosomeFactory()
        {
            //This is a lis of all of the possible functions that can be run, which will take in a list and return a function, which will then take a string and return a list from that string
            //The end result is that the ChromosomeFactories allows a list and a function that can be run once a given item in the list has been chosen by the Chromosome
            //Here we use expression lambdas: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
            //which allow us to specify some input parameters for the function. Even if they're not used obviously they are being passed into each lambda expression and thus must still be defined
            _chromosomeFactories = new List<Func<List<string>, Func<string, List<object>>, Chromosome>> {
                (validColumns, validDataGetter) => new Projection(validColumns),
                (validColumns, validDataGetter) => new Selection(validColumns, validDataGetter)
                //(validColumns, validDataGetter) => new GroupBy(validDataGetter);
                //add the other types here
            };
        }

        protected static List<Func<List<String>, Func<string, List<object>>, Chromosome>> _chromosomeFactories;

        
        protected abstract StubIndividual CrossWithSpouse(StubIndividual spouse);
        public StubIndividual Cross(StubIndividual spouse)
        {
            if(GetType() != spouse.GetType())
            {
                throw new InvalidOperationException();
            }
            return CrossWithSpouse(spouse);
        }

        public abstract void Mutate();
        public abstract string ToSql();
    }
}
