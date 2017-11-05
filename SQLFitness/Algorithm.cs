using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SQLFitness
{
    class Algorithm
    {
        IFitness _selector;
        private Population _population;
        private Population _matingPool;

        private DBAccess _db;

        public Population BestIndividuals { get; set; }
        private StreamWriter _file;
        private int _generation;

        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        /// <param name="db">Database which the algorithm is going to be run on</param>
        public Algorithm(DBAccess db)
        {
            //Setup params for most of the class here:
            BestIndividuals = new Population(_selector);
            _selector = new ClientFitness();
            _population = new Population(db.ValidColumnGetter(), db.ValidDataGetter, _selector);
            _matingPool = new Population(_selector);
            _db = db;
            _file = new StreamWriter(Utility.FitnessFile);
            _file.AutoFlush = true;
            _generation = 1;
        }

        private void _selection()
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

        private void _crossover()
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
            _population.AddRange(new Population(_db.ValidColumnGetter(), _db.ValidDataGetter, _selector, Utility.PopulationSize - _population.Count));
            _matingPool = new Population(_selector);
            if (_matingPool.Count != 0) { throw new IndexOutOfRangeException(nameof(_matingPool) + " not zero"); }
        }

        private void _evaluation()
        {
            Parallel.ForEach(_population, new ParallelOptions { MaxDegreeOfParallelism = 1 }, (x) =>
            {
                if (x.Fitness == null)
                {
                    //This is a good example of using OO to prevent things from happening by using objects and our custom type
                    //Example of encapsulation - information hiding
                    x.Fitness = new Fitness(new ClientFitness().Evaluate(x));
                    Console.WriteLine("Processing {0} on thread {1}", x.Fitness.Value, Thread.CurrentThread.ManagedThreadId);
                }
                else
                {
                    Console.WriteLine("Already Set!");
                }
            });
        }

        private void _mutate() => _population.GetRandomValue().Mutate();

        //Talk through this design
        public void Evolve()
        {
            
            Console.WriteLine(nameof(_evaluation));
            _evaluation();
            Console.WriteLine(nameof(_selection));
            _selection();

            this.BestIndividuals.Add(_population[0]);
            var line = String.Join(",", _population.Select(x => x.Fitness.Value.ToString()).ToArray());
            _file.WriteLine($"{_generation},{line}");
            
            Console.WriteLine(BestIndividuals);

            Console.WriteLine(nameof(_crossover));
            _crossover();
            Console.WriteLine(nameof(_mutate));
            _mutate();
            _generation++;
        }
    }
}
