using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SQLFitness
{
    public class Interpreter
    {
        private StubIndividual _sqlIndividual;
        private readonly string _tableName;

        //This object is required to communicate with the DBAccess to get a subset of the data back
        //This object will know about SQL Syntax. An interpreter is essentially a kind of reverse parser, so It'll be passed in an individual which it will then convert into something usable
        public Interpreter () : this(Utility.TableName)
        {
        }

        public Interpreter( string tableName)
        {
            _tableName = tableName;
        }

        public string Parse(StubIndividual sqlIndividual)
        {
            _sqlIndividual = sqlIndividual;
            return sqlIndividual.ToSql();
        }
    }
}
