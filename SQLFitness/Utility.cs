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
        public static T GetRandomValue<T>(this List<T> list) => list[GetRandomNum(list.Count)];

        //Note that const fields are always static
        public const string ConnString = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
        public const string TableName = "Country";
    }
}
