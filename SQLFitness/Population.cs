using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Population
    {
        //Fix this so that population has behaviour and attributes that make sense for a population

        //A population needs to be able to be added to another existing population in place
        public List<Individual> _individuals;
        public Population(int n)
        {
            //Generate a population of indivuduals of size n

        }

        public Population() : this(100)
        {

        }
    }
}
