using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SQLFitness
{
    public abstract class LoggingAlgorithm
    {
        protected Population _population;

        public Population BestIndividuals { get; set; }
        public int Generation { get; private set; }

        private readonly StreamWriter _file;
        private readonly StreamWriter _bestIndividuals;
        /// <summary>
        /// Most of the rules for a GA implementation need to be here. Most of the other parts should be relatively loosely coupled to a specific implementation or set of parameters
        /// </summary>
        protected LoggingAlgorithm()
        {
            //Setup params for most of the class here:
            this.BestIndividuals = new Population();
            _file = new StreamWriter(Utility.FitnessFile)
            {
                AutoFlush = true
            };
            _bestIndividuals = new StreamWriter(Utility.IndividualFile)
            {
                AutoFlush = true
            };

            this.Generation = 1;
            _file.WriteLine($"Generation, Evolve Time, Evaluation Time, Selection Time, Crossover Time, Mutation Time, Individuals");
        }

        abstract protected void _selection();
        abstract protected void _crossover();
        abstract protected void _evaluation();
        abstract protected void _mutation();

        //Talk through this design
        public void Evolve()
        {
            var evolveStopWatch = new Stopwatch();
            evolveStopWatch.Start();
            var evaluationStopWatch = new Stopwatch();
            var selectionStopWatch = new Stopwatch();
            var crossoverStopWatch = new Stopwatch();
            var mutationStopWatch = new Stopwatch();

            evaluationStopWatch.Start();
            //Console.WriteLine(nameof(_evaluation));
            _evaluation();
            evaluationStopWatch.Stop();
            selectionStopWatch.Start();
            //Console.WriteLine(nameof(_selection));
            _selection();
            selectionStopWatch.Stop();
            var line = String.Join(",", _population.Select(x => x.Fitness.Value.ToString()).ToArray());

            if (this.BestIndividuals.Count == 0)
            {
                this.BestIndividuals.Add(_population[0]);
            }

            if (this.BestIndividuals.Count > 0 && this.BestIndividuals.Last() != _population[0])
            {
                this.BestIndividuals.Add(_population[0]);
                //Code Super unoptimised, slows entire program

                //Console.WriteLine($"Best Individuals:\n {String.Join("\n ", this.BestIndividuals.Select( x=> $"{x.ToSql()}, {x.Fitness.Value}"))}\n");
                String temp = "\"" + this.BestIndividuals.Last().ToSql().Replace("\n","") + "\"" + "," + "\"" + this.BestIndividuals.Last().Fitness.Value + "\"";
                _bestIndividuals.WriteLine(temp);
            }
            _file.WriteLine($"{this.Generation}, {evolveStopWatch.Elapsed}, {evaluationStopWatch.Elapsed}, {selectionStopWatch.Elapsed}, {crossoverStopWatch.Elapsed}, {mutationStopWatch.Elapsed}, {line}");


            crossoverStopWatch.Start();
            //Console.WriteLine(nameof(_crossover));
            _crossover();
            crossoverStopWatch.Stop();
            mutationStopWatch.Start();
            //Console.WriteLine(nameof(_mutation));
            _mutation();
            mutationStopWatch.Stop();
            this.Generation++;
            evolveStopWatch.Stop();

        }
    }
}
