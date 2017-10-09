using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    class Algorithm
    {
        double _matingProp = 0.5;

        private Population _population;
        private Population _matingPool;

        private DBAccess _db;
        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        /// <param name="db">Database which the algorithm is going to be run on</param>
        public Algorithm(DBAccess db)
        {
            //Create a population
            _population = new Population(db.ValidColumnGetter(), db.ValidDataGetter);
            _matingPool = new Population() { };
            _db = db;
        }

        private void _selection(IFitness selector)
        {
            //Assigns a fitness to each individual
            foreach (var individual in _population)
            {
                individual.Fitness = selector.Evaluate(individual);
            }

            //Orders by the fitness
            //Something needs to sort it
            _population.Sort();
            //Create a mating pool from the best n proportion
            double _matingProp = 0.5;
            for (var i = 0; i < _population.Count * _matingProp; i++)
            {
                _matingPool.Add(_population[i]);
            }
        }

        private void _crossover()
        {
            //Cross one with the other and add the result to to pool, untill the last size of the mating pool is reached.

            while (_matingPool.Count < _population.Count)
            {
                //Pick two random individuals

                var i1 = _matingPool.GetRandomValue();
                var i2 = _matingPool.GetRandomValue();
                _matingPool.Add(i1.Cross(i2));
                _matingPool.Add(i2.Cross(i1));
            }
        }

        private void _mutate(Population mutatablePopulation)
        {

        }

        //Talk through this design
        public void Evolve()
        {
            _selection(new DBSelector(_db));
            _crossover();
            //_mutate();
            _population = _matingPool;
        }
    }
}
