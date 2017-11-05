using System;
using System.Collections.Generic;
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

        //Note that const fields are always static
        public const string ConnString = "server=localhost;user=root;password=example;database=species;port=3306;sslmode=none";
        public const string TableName = "insectdiscoveries";

        //public const string ConnString = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
        //public const string TableName = "country";

        //Mating pools
        public const double MatingProportion = 0.7;
        public const int PopulationSize = 50;

        //Fitness server settings
        public const string FitnessServerAddress = "127.0.0.1";
        public const int FitnessServerPort = 1506;
    }
}
