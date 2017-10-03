using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class DBSelector : IFitness
    {
        /// <summary>
        /// A DBSelector is what allows selection to be done on an individual, so a selection implements a fitness function
        /// </summary>
        private DBAccess _db;
        public DBSelector(DBAccess db)
        {
            _db = db;
        }
        public double Evaluate(Individual individual) => throw new NotImplementedException();
    }
}
