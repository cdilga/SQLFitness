using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace SQLFitness
{
    static class Program
    {
        static void Main(string[] args)
        {
            //Create a dbaccess
#if !DEBUG
            var basicGA = new TreeTestAlgorithm(new PerformanceTestFitness());
#else
            var db = new DBAccess();
            var basicGA = new TreeSelectionAlgorithm(db, new ClientFitness());
#endif
            for (var i = 0; i < Utility.MaxIterations; i++)
            {
                basicGA.Evolve();
                Console.WriteLine($"{i}");
            }
            //db.Close();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}