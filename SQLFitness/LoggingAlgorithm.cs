﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SQLFitness
{
    public abstract class LoggingAlgorithm
    {
        protected Population _population;

        public Population BestIndividuals { get; set; }
        public int Generation { get; private set; }
        
        private readonly StreamWriter _file;

        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        /// <param name="db">Database which the algorithm is going to be run on</param>
        /// <param name="selector"></param>
        protected LoggingAlgorithm()
        {
            //Setup params for most of the class here:
            this.BestIndividuals = new Population();
            _file = new StreamWriter(Utility.FitnessFile)
            {
                AutoFlush = true
            };
            
            this.Generation = 1;
        }

        abstract protected void _selection();
        abstract protected void _crossover();
        abstract protected void _evaluation();
        abstract protected void _mutate();

        //Talk through this design
        public void Evolve()
        {
            Console.WriteLine(nameof(_evaluation));
            _evaluation();
            Console.WriteLine(nameof(_selection));
            _selection();

            this.BestIndividuals.Add(_population[0]);
            var line = String.Join(",", _population.Select(x => x.Fitness.Value.ToString()).ToArray());
            _file.WriteLine($"{this.Generation},{line}");
            
            Console.WriteLine($"Best Individuals: {String.Join(", ", this.BestIndividuals)}");

            Console.WriteLine(nameof(_crossover));
            _crossover();
            Console.WriteLine(nameof(_mutate));
            _mutate();
            this.Generation++;
        }
    }
}
