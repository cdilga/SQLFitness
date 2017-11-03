using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace SQLFitness
{
    public class FlatIndividual : StubIndividual
    {
        private Chromosome[] Genome { get; }
        public FlatIndividual(List<String> validColumns, Func<string, List<object>> validDataGetter)
            : this(Enumerable.Range(0, 5).Select(i => _generateRandomChromosome(validColumns, validDataGetter)))
        {
        }

        private FlatIndividual(IEnumerable<Chromosome> genome)
        {
            Genome = genome.ToArray();
        }

        protected override StubIndividual CrossWithSpouse(StubIndividual spouse)
        {
            var flatSpouse = (FlatIndividual)spouse;
            //There are several cases here - we have two children, or we have one starting with this's genome, or we have one ending with this's genome
            //Cut at random point along this

            var newChromosome = new List<Chromosome>();
            for (var i = 0; i < Utility.GetRandomNum(this.Genome.Length); i++)
            {
                newChromosome.Add(this.Genome[i]);
            }
            for (var i = Utility.GetRandomNum(flatSpouse.Genome.Length); i < flatSpouse.Genome.Length; i++)
            {
                newChromosome.Add(flatSpouse.Genome[i]);
            }
            //Cut at random point along them
            return new FlatIndividual(newChromosome);
        }

        public override void Mutate()
        {
            this.Fitness = null;
            this.Genome.GetRandomValue().Mutate();
        }

        private IEnumerable<Chromosome> _selectType(IEnumerable<Chromosome> chromosomes, Type type) => chromosomes.Where(c => c.GetType() == type);
        //{
        //    var returnList = new List<Chromosome>();
        //    foreach (var chromosome in chromosomes)
        //    {
        //        if (chromosome.GetType() == type)
        //        {
        //            returnList.Add(chromosome);
        //        }
        //    }
        //    return returnList;
        //}

        public override string GenerateSql()
        {
            var chromosomes = this.Genome;
            var _projections = chromosomes.OfType<Projection>();
            var _selections = chromosomes.OfType<Selection>();
            var tempSelections = new List<string>();
            //foreach (var selection in _selections)
            //{
            //    tempSelections.Add($"\"{selection}\"");
            //}

            var catenatedProjections = String.Join(", ", _projections.Select(x => $"`{x}`"));
            var selectComponent = catenatedProjections.Any() ? catenatedProjections : "*";
            var query = $"SELECT { selectComponent } FROM { Utility.TableName }";

            var catenatedSelections = String.Join(" AND ", _selections);
            if (catenatedSelections.Any())
            {
                query += $" WHERE {catenatedSelections}";
            }

            //var catenatedGrouping = String.Join(" ", _groups);
            // if (catenatedSelections.Any())
            //{
            //    query += $"GROUP BY {catenatedGrouping}";
            //}
            return query + ";\n";
        }

        private static Chromosome _generateRandomChromosome(List<String> validColumns, Func<string, List<object>> validDataGetter)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
            //This is useful because we're essentially - in our utility class - extending the IEnumerable types but without having to inherit a base class
            var random = Utility.GetRandomNum(2);
            return _chromosomeFactories[random].Invoke(validColumns, validDataGetter);
            //The .Invoke is used as an alternative to putting GetRandomValue()(validColumns, validDataGetter);
            //Initialise it
            //return the new instance
        }
    }
}
