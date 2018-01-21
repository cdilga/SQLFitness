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
            var db = new DBAccess();
            var basicGA = new TreeSelectionAlgorithm(db, new ClientFitness());
            for (var i = 0; i < 10000; i++)
            {
                basicGA.Evolve();
                Console.WriteLine($"{i}");
            }
            db.Close();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}