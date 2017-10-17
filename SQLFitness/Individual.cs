using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SQLFitness
{
    public class Individual
    {
        public Fitness Fitness { get; set; }
        public List<Chromosome> Genome { get; set; } = new List<Chromosome>();
        private List<String> _validColumns;
        private Func<string, List<object>> _validDataGetter;

        public Individual(List<String> validColumns, Func<string, List<object>> validDataGetter)
        {
            _validColumns = validColumns;
            _validDataGetter = validDataGetter;

            const int length = 5;
            for (var i = 0; i < length; i++)
            {
                this.Genome.Add(_generateRandomChromosome(validColumns, validDataGetter));
                //From a list of Chromosomes that we can initialise 
                //Plus something
            }
        }

        //Setup a static constructor that is run only once whenever the first individual is initialised
        static Individual()
        {
            _initialiseChromosomeFactory();
        }

        private static void _initialiseChromosomeFactory()
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

        private static List<Func<List<String>, Func<string, List<object>>, Chromosome>> _chromosomeFactories;

        //Only instances of individuals can access this
        private static Chromosome _generateRandomChromosome(List<String> validColumns, Func<string, List<object>> validDataGetter)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
            //This is useful because we're essentially - in our utility class - extending the IEnumerable types but without having to inherit a base class
            var random = Utility.GetRandomNum();
            return _chromosomeFactories[random<=50? 0: 1 ].Invoke(validColumns, validDataGetter);
            //The .Invoke is used as an alternative to putting GetRandomValue()(validColumns, validDataGetter);
            //Initialise it
            //return the new instance
        }

        public Individual Cross(Individual spouse)
        {
            //There are several cases here - we have two children, or we have one starting with this's genome, or we have one ending with this's genome
            var newIndividual = new Individual(_validColumns, _validDataGetter);
            //Cut at random point along this
            
            for (var i = 0; i < Utility.GetRandomNum(this.Genome.Count); i++)
            {
                newIndividual.Genome.Add(this.Genome[i]);
            }
            for (var i = Utility.GetRandomNum(spouse.Genome.Count); i < spouse.Genome.Count; i++)
            {
                newIndividual.Genome.Add(spouse.Genome[i]);
            }
            //Cut at random point along them
            return newIndividual;
        }
    }
}
