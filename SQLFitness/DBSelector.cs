using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class DBSelector : ISelector
    {
        private DBAccess _db;
        public DBSelector(DBAccess db)
        {
            _db = db;
        }
        public double evaluate(Individual individual) => throw new NotImplementedException();
    }
}
