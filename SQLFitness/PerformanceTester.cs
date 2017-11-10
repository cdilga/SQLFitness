using System.Collections.Generic;

namespace SQLFitness
{
    internal static class PerformanceTester
    {
        private static int n = 100;
        private static int m = 100;
        private static List<string> _validColumns = new List<string>(n);
        private static List<object> _validData = new List<object>(m);
        static PerformanceTester()
        {
            for (var i = 0; i < n; i ++)
            {
                _validColumns.Add($"Column {i}");
            }
            for (var i = 0; i < m; i++)
            {
                _validData.Add($"Data {i}");
            }
        }

        public static List<string> ValidColumnGetter()
        {
            return _validColumns;
        }

        public static List<object> ValidDataGetter(string str)
        {
            return _validData;
        }
    }
}