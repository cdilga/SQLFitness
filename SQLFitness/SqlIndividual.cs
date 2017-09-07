using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class SqlIndividual : Individual
    {
        public SqlIndividual(int chromosomes) : base(chromosomes)
        {
            //setup some mysql stuff here, connections and stuff
        }

        //TODO MySQL properties (wait untill we know how to interface with DB better)
    }
}
