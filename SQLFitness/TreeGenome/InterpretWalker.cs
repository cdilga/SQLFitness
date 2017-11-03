using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome
{
    class InterpretWalker : Visitor
    {
        private readonly StringBuilder _sqlBuilder;

        public InterpretWalker()
        {
            _sqlBuilder = new StringBuilder("WHERE ");
        }
    }
}
