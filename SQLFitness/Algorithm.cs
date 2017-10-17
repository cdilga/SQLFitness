using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    class Algorithm
    {
        double _matingProp = 0.5;
        IFitness _selector;
        private Population _population;
        private Population _matingPool;

        private DBAccess _db;
        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        /// <param name="db">Database which the algorithm is going to be run on</param>
        public Algorithm(DBAccess db)
        {
            //Setup params for most of the class here:
            _selector = new DBSelector(db, new Interpreter(db.TableName));
            _population = new Population(db.ValidColumnGetter(), db.ValidDataGetter, _selector);
            _matingPool = new Population(_selector);
            _db = db;
        }

        private void _selection()
        {
            //Assigns a fitness to each individual
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
            //clear out the population
            _population = new Population(_selector);
            //Cross one with the other and add the result to to pool, untill the last size of the mating pool is reached.

            while (_population.Count < _matingPool.Count)
            {
                //Pick two random individuals

                var i1 = _matingPool.GetRandomValue();
                var i2 = _matingPool.GetRandomValue();
                var tempChild1 = i1.Cross(i2);
                var tempChild2 = i2.Cross(i1);
                _population.Add(tempChild1);
                _population.Add(tempChild2);
            }
        }

        private void _mutate(Population mutatablePopulation)
        {

        }

        //Talk through this design
        public void Evolve()
        {
            _selection();
            _crossover();
            //_mutate();
            _population = _matingPool;
        }
    }
}
