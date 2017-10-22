using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Population : List<Individual>
    {
        //Forming consistent abstractions here
        //This one fitness must be thread safe
        private IFitness _fitness;
        //A population needs to be able to be added to another existing population in place
        public Population(List<string> validColumnData, Func<string, List<object>> validRowDataGetter, IFitness fitnessFunc, int n = 50) : base()
        {
            _fitness = fitnessFunc;
            //Generate a population of indivuduals of size n
            for (var i = 0; i < n; i++)
            {
                this.Add(new Individual(validColumnData, validRowDataGetter));
            }
        }
        //TODO remember to assign a new fitness on mutation

        public Population(IEnumerable<Individual> basePopulation, IFitness fitnessFunc) : base(basePopulation) { _fitness = fitnessFunc; }
        public Population(IFitness fitnessFunc) : base() { _fitness = fitnessFunc; }

        public double Evaluate(Individual item)
        {
            Console.WriteLine(item.ToString());
            return _fitness.Evaluate(item);
        }

        new public void Sort()
        {
            Sort((Individual x, Individual y) => x.Fitness.Value.CompareTo(y.Fitness.Value));
        }
    }
}
