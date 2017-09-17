using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SQLFitness
{
    public class Individual
    {
        public List<IChromosome> Genome { get; set; } = new List<IChromosome>();
        //Make this a private constructor so that only an individual can care about how long new individual is 

        public Individual(List<String> validColumns, Func<string, List<object>> validDataGetter)
        {
            const int length = 50;
            for (var i = 0; i < length; i++)
            {
                this.Genome.Add(_generateRandomChromosome(validColumns, validDataGetter));
                //From a list of IChromosomes that we can initialise 
                //Plus something
            }
        }

        //Setup a static constructor that is run only once whenever the first individual is initialised
        static Individual()
        {
            InitialiseIChromosomeFactory();
        }
        private static void InitialiseIChromosomeFactory()
        {
            //This is a lis of all of the possible functions that can be run, which will take in a list and return a function, which will then take a string and return a list from that string
            //The end result is that the IChromosomeFactories allows a list and a function that can be run once a given item in the list has been chosen by the IChromosome
            //Here we use expression lambdas: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
            //which allow us to specify some input parameters for the function. Even if they're not used obviously they are being passed into each lambda expression and thus must still be defined
            IChromosomeFactories = new List<Func<List<string>, Func<string, List<object>>, IChromosome>> {
                (validColumns, validDataGetter) => new Projection(validColumns),
                (validColumns, validDataGetter) => new Selection(validColumns, validDataGetter)
                //(validColumns, validDataGetter) => new GroupBy(validDataGetter);
                //add the other types here
            };
        }

        private static List<Func<List<String>, Func<string, List<object>>, IChromosome>> IChromosomeFactories;

        //Only instances of individuals can access this
        private static IChromosome _generateRandomChromosome(List<String> validColumns, Func<string, List<object>> validDataGetter)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
            //This is useful because we're essentially - in our utility class - extending the IEnumerable types but without having to inherit a base class
            return IChromosomeFactories.GetRandomValue().Invoke(validColumns, validDataGetter);
            //The .Invoke is used as an alternative to putting GetRandomValue()(validColumns, validDataGetter);
            //Initialise it
            //return the new instance
        }
    }
}
