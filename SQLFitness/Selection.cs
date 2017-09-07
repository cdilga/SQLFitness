using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Selection
    {
        private enum Operator { equal, notEqual, greaterThan, lessThan, greaterThanEqual, lessThanEqual }

        public Selection ()
        {
            //Has an operator
            //Has a condition (might be a numerical value or something) - sourced from the database
            //Has a value (i.e. attribute name)
        }

        
    }
}
