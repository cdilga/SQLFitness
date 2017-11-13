using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLFitness
{
    public static class Utility
    {
        private static Random _randomGenerator = new Random(4); //4 is guarenteed to be random - picked from random.org

        /// <summary>
        /// Gets a random number between <paramref name="min"/> and <paramref name="max"/>
        /// </summary>
        /// <param name="min">The minimum number INCLUSIVE</param>
        /// <param name="max">The maximum number EXCLUSIVE</param>
        /// <returns>A random number</returns>
        public static int GetRandomNum(int min, int max) => _randomGenerator.Next(min, max);

        /// <summary>
        /// Gets a random number between 0 and <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum number EXCLUSIVE</param>
        /// <returns>A random number</returns>
        public static int GetRandomNum(int max = 100) => _randomGenerator.Next(max);
        //So cool when someone told me to look this up:
        public static T GetRandomValue<T>(this List<T> list) => list[GetRandomNum(list.Count)];
        public static T GetRandomValue<T>(this T[] list) => list[GetRandomNum(list.Length)];
        public static List<Chromosome> DistinctChromosomes(this List<Chromosome> list) => list.GroupBy(o => o.Field).Select(c => c.First()).ToList();
        public static List<Selection> DistinctChromosomes(this List<Selection> list) => list.GroupBy(o => o.Field).Select(c => c.First()).ToList();
        public static List<Projection> DistinctChromosomes(this List<Projection> list) => list.GroupBy(o => o.Field).Select(c => c.First()).ToList();
        public static Projection[] DistinctChromosomes(this Projection[] list) => list.ToList().GroupBy(o => o.Field).Select(c => c.First()).ToArray();

        public static string ToSQL(this PredicateType condition)
        {
            switch (condition)
            {
                case PredicateType.LessThan: return "<";
                case PredicateType.GreaterThan: return ">";
                case PredicateType.Equal: return "=";
                case PredicateType.NotEqual: return "<>";
                case PredicateType.GreaterThanEqual: return ">=";
                case PredicateType.LessThanEqual: return "<=";
                default: throw new ArgumentException(nameof(condition));
            }
        }

        public static string ToSql(this BinaryNodeType nodeType)
        {
            switch (nodeType)
            {
                case BinaryNodeType.AND: return "AND";
                case BinaryNodeType.OR: return "OR";
                default: throw new ArgumentException(nameof(nodeType));
            }
        }

        /// <summary>
        /// File which the best individuals fitness is written to at each iteration.
        /// </summary>
        public const string FitnessFile = "FitnessValues.csv";
        public const string IndividualFile = "BestIndividuals.csv";


        //Note that const fields are always static
        public const string ConnString = "server=localhost;user=root;password=example;database=species;port=3306;sslmode=none";
        public const string TableName = "insectdiscoveries";

        //public const string ConnString = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
        //public const string TableName = "country";

        //Mating pools
        //Probably shouldn't be above 0.5
        public const double MatingProportion = 0.4;
        public const double MutationProportion = 0.1;
        public const int PopulationSize = 100;

        //ChromosomeSettings

        public const int FlatChromosomeLength = 10;
        public const int TreeChromosomeBranchSize = 5;
        public const int MaxTreeChromosomeProjectionSize = 5;

        //Fitness server settings
        public const string FitnessServerAddress = "127.0.0.1";
        public const int FitnessServerPort = 1506;

        public static void TestDuplicates(List<Projection> list)
        {
            foreach (var item in list)
            {
                var sublist = new List<Projection>(list);
                sublist.Remove(item);
                Projection predicateItem = (Projection)item;
                foreach (var matchItem in sublist)
                {
                    Projection matchPredicate = (Projection)matchItem;
                    if (predicateItem.Field == matchPredicate.Field) throw new ArgumentException("Invalid field, is duplicate");
                    if (predicateItem.ToString() == matchPredicate.ToString()) throw new ArgumentException("Invalid field, is duplicate by string");
                }
            }
        }
    }
}
