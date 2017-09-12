using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace SQLFitness
{
    static class Program
    {
        static void Main(string[] args)
        {
            //connect to a mysqldb and execute an example query

            var connStr = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
            var conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting");
                conn.Open();
                var sql = "SELECT * FROM country";
                var cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                var columns = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++) {
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
            Console.ReadLine();

        }
    }
}