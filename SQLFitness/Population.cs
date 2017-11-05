using System;
using System.Collections.Generic;

namespace SQLFitness
{
    public class Population : List<StubIndividual>
    {
        //Forming consistent abstractions here
        //This one fitness must be thread safe
        private IFitness _fitness;
        //A population needs to be able to be added to another existing population in place
        public Population(List<string> validColumnData, Func<string, List<object>> validRowDataGetter, IFitness fitnessFunc, int n = Utility.PopulationSize) : base()
        {
            _fitness = fitnessFunc;
            //Generate a population of indivuduals of size n
            for (var i = 0; i < n; i++)
            {
                this.Add(new FlatIndividual(validColumnData, validRowDataGetter));
            }
        }
        //TODO remember to assign a new fitness on mutation

        public Population(IEnumerable<StubIndividual> basePopulation, IFitness fitnessFunc) : base(basePopulation) { _fitness = fitnessFunc; }
        public Population(IFitness fitnessFunc) { _fitness = fitnessFunc; }
        public Population() : base() { }

        public double Evaluate(StubIndividual item)
        {
            Console.WriteLine(item.ToString());
            return _fitness.Evaluate(item);
        }

        new public void Sort()
        {
            Sort((StubIndividual x, StubIndividual y) => y.Fitness.Value.CompareTo(x.Fitness.Value));
        }
    }
}
