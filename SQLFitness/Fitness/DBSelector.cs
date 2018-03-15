using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class DBSelector
    {
        /// <summary>
        /// A DBSelector is what allows selection to be done on an individual, so a selection implements a fitness function
        /// </summary>
        private DBAccess _db;
        private Interpreter _interpreter;
        const string connStr = Utility.ConnString;
        //private MySqlConnection conn = new MySqlConnection(connStr);

        public DBSelector(DBAccess db, Interpreter interpreter)
        {
            _interpreter = interpreter;
            _db = db;
        }

        public double Evaluate(StubIndividual individual) => throw new ArgumentNullException("Stub individual had no fitness before it was evaluated.");
    }
}
