using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Population
    {
        //Fix this so that population has behaviour and attributes that make sense for a population

        //A population needs to be able to be added to another existing population in place
        private List<Individual> _individuals { get; }
        public Population(List<string> validColumnData, Func<string, List<object>> validRowDataGetter, int n = 50)
        {
            //Generate a population of indivuduals of size n
            for (var i = 0; i < n; i++)
            {
                _individuals.Add(new Individual(validColumnData, validRowDataGetter));
            }
        }

        public Population(List<Individual> individuals)
        {
            _individuals = individuals;
        }

        public void Add(Individual newIndividual) => this._individuals.Add(newIndividual);
        public int Count {get => this._individuals.Count;}
    }
}
