using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    static class Utility
    {
        private static Random _randomGenerator = new Random();

        public static int GetRandomNum(int min, int max) => _randomGenerator.Next(min, max);
        public static int GetRandomNum(int max = 100) => _randomGenerator.Next(max);
        //So cool when someone told me to look this up:
        public static T GetRandomValue<T>(this List<T> list) => list[GetRandomNum(list.Count - 1)];

        //Note that const fields are always static
        //public const string ConnString = "server=localhost;user=root;password=example;database=species;port=3306;sslmode=none";
        //public const string TableName = "insectdiscoveries";

        public const string ConnString = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
        public const string TableName = "country";

        //Mating pools
        public const double MatingProportion = 0.5;

        //Fitness server settings
        public const string FitnessServerAddress = "localhost";
        public const int FitnessServerPort = 44404;
    }
}
