using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    class Algorithm
    {
        private Population _population { get; set; }
        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        public Algorithm()
        {
            //Create a population
            _population = new Population();
        }

        private void _selection(Func<Individual, float> fitness)
        {
            //Assigns a fitness to each individual
            //Orders by the fitness
            //Throws out some number of individuals from the population
            //Create a new population from 
        }

        private void _crossover()
        {

        }

        private void _mutate()
        {

        }

        public void Evolve()
        {
            _selection();
            _crossover();
            _mutate();
        }
    }
}
