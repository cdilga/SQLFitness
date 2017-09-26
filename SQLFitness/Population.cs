using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Population : List<Individual>
    {
        //Forming consistent abstractions here

        //A population needs to be able to be added to another existing population in place
        public Population(List<string> validColumnData, Func<string, List<object>> validRowDataGetter, int n = 50) : base()
        {
            //Generate a population of indivuduals of size n
            for (var i = 0; i < n; i++)
            {
                this.Add(new Individual(validColumnData, validRowDataGetter));
            }
        }

        public Population(IEnumerable<Individual> basePopulation) : base(basePopulation) { }
        public Population() : base() { }

        new public void Sort()
        {
            Sort((Individual x, Individual y) => (int)((x.Fitness - y.Fitness) / Math.Pow(x.Fitness - y.Fitness, 2)));
        }
    }
}
