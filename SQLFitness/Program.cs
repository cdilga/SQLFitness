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
            
            Console.ReadLine();

        }
    }
}