using System;
using MySql.Data.MySqlClient;

namespace SQLFitness
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //connect to a mysqldb and execute an example query

            Tutorial1 test = new Tutorial1();

            Console.ReadLine();
        }
    }
}