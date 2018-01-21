using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SQLFitness
{
    class FlatSelectionAlgorithm : LoggingAlgorithm
    {
        private Population _matingPool;

        private DBAccess _db;
        private readonly IFitness _selector;
        private Func<List<string>, Func<string, List<object>>, FlatIndividual> _flatFactory;
        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        /// <param name="db">Database which the algorithm is going to be run on</param>
        /// <param name="selector"></param>
        public FlatSelectionAlgorithm(DBAccess db, IFitness selector) : base()
        {
            //Setup params for most of the class here:
            _matingPool = new Population(_selector);
            _db = db;
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _flatFactory = (validColumn, validData) => new FlatIndividual(db.ValidColumnGetter(), db.ValidDataGetter);
            _population = new Population(db.ValidColumnGetter(), db.ValidDataGetter, _selector, _flatFactory);
        }

        protected override void _selection()
        {
            //Assigns a fitness to each individual
            //Orders by the fitness
            //Something needs to sort it
            _population.Sort();
            //Create a mating pool from the best n proportion
            var max = _population.Count * Utility.MatingProportion;
            for (var i = 0; i < max - max%2; i++)
            {
                _matingPool.Add(_population[i]);
            }
        }

        protected override void _crossover()
        {
            //clear out the population
            //Cross one with the other and add the result to to pool, untill the last size of the mating pool is reached.
            _population = new Population();
            
            var numToCrossProduce = Utility.PopulationSize * Utility.MatingProportion;
            var initialPopCount = _population.Count;
            while (_population.Count < numToCrossProduce)
            {
                //Pick two random individuals
                var i1 = _matingPool.GetRandomValue();
                var i2 = _matingPool.GetRandomValue();
                var tempChild1 = i1.Cross(i2);
                var tempChild2 = i2.Cross(i1);
                _population.Add(tempChild1);
                _population.Add(tempChild2);
            }
            //keeps the best parents
            _population.AddRange(_matingPool);
            _population.AddRange(new Population(_db.ValidColumnGetter(), _db.ValidDataGetter, _selector, _flatFactory, Utility.PopulationSize - _population.Count));
            _matingPool = new Population(_selector);
            if (_matingPool.Count != 0) { throw new IndexOutOfRangeException(nameof(_matingPool) + " not zero"); }
        }

        protected override void _evaluation()
        {
            Parallel.ForEach(_population, new ParallelOptions { MaxDegreeOfParallelism = 8 }, (x) =>
            {
                if (x.Fitness == null)
                {
                    //This is a good example of using OO to prevent things from happening by using objects and our custom type
                    //Example of encapsulation - information hiding
                    //TODO fix reflection in high performance code
                    x.Fitness = _selector.Evaluate(x)[0];
                    Console.WriteLine("Returned {0} on thread {1}", x.Fitness, Thread.CurrentThread.ManagedThreadId);
                }
                else
                {
                    Console.WriteLine("Already Set!");
                }
            });
        }

        protected override void _mutation()
        {
            for (var i = 0; i < _population.Count * Utility.MutationProportion; i++)
            {
                _population.GetRandomValue().Mutate();
            }
        }

    }
}
