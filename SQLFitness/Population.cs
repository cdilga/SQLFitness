using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Population : List<Individual>
    {
        //Forming consistent abstractions here
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

        public new void Add(Individual item)
        {
            if (item.Fitness == null)
            {
                //This is a good example of using OO to prevent things from happening by using objects and our custom type
                //Example of encapsulation - information hiding
                item.Fitness = new Fitness(_fitness.Evaluate(item));
            }
            base.Add(item);
        }
        public Population(IEnumerable<Individual> basePopulation, IFitness fitnessFunc) : base(basePopulation) { _fitness = fitnessFunc; }
        public Population(IFitness fitnessFunc) : base() { _fitness = fitnessFunc; }

        new public void Sort()
        {
            Sort((Individual x, Individual y) => x.Fitness.Value.CompareTo(y.Fitness.Value));
        }
    }
}
