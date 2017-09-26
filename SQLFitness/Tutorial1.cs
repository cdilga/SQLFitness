using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace SQLFitness
{
    class Tutorial1
    {
        public Tutorial1()
        {
            //For some reason the DotNetCore implementation of the ADO SQL adapter? does not support SSL at this stage...
            var connStr = "server=localhost;user=root;password=example;database=world;port=3306;sslmode=none";
            var conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting");
                conn.Open();
                _listPeople(conn);
                //_insertCountry(conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            conn.Close();
            Console.WriteLine("Done.");
        }

        private void _insertCountry(MySqlConnection conn)
        {
            var sql = "INSERT INTO country (name, headofstate, continent) VALUES ('Disneyland', 'Mickey Mouse', 'North America')";
            var cmd = new MySqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
        private void _listPeople(MySqlConnection conn)
        {
            try
            {
                var sql = "SELECT name, headofstate FROM country WHERE continent='North America'";
                var cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader[0] + " -- " + reader[1]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
