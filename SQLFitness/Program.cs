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
            var basicGA = new Algorithm(db);
            for (var i = 0; i < 100; i++)
            {
                basicGA.Evolve();
                Console.WriteLine($"===========\nIteration: {i}");
            }
            db.Close();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void Legacy()
        {
            const string connStr = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
            var conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting");
                conn.Open();
                var sql = "SELECT * FROM country";
                var cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                var columns = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                    Console.WriteLine(reader.GetName(i));
                }
                //var testProjection = new Projection(reader);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");

            Func<string, List<object>> dataGetter = x => new List<object> { "Data 1", "Data 2", "Data 3" };
            List<string> data = new List<string> { "Column 1", "Column 2", "Column 3", "Column 4" };


            for (var i = 0; i < 30; i++)
            {
                var testInterpreter = new Interpreter("country");
                Console.WriteLine(testInterpreter.Parse(new FlatIndividual(data, dataGetter)));
            }

            var db = new DBAccess();
            db.ValidColumnGetter();
            Console.ReadLine();
        }
    }
}