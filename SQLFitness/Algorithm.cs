using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    class Algorithm
    {
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
            _db = db;
        }

        private void _selection(ISelector selector)
        {
            //Assigns a fitness to each individual
            //Orders by the fitness
            //Create a mating pool from the best
            //Some mechanism needs to assign a fitness to each one
            //Something needs to sort it
        }

        private void _crossover()
        {

        }

        private void _mutate()
        {

        }

        public void Evolve()
        {
            _selection(new DBSelector(_db));
            _crossover();
            _mutate();
        }
    }
}
