using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public interface IChromosome
    {
        void Mutate();
        //A property must exist that is then used by the other classes to generate MySQL queries, perferably 
        //non stringy so that it can be easily swapped out with another language
        //TODO Investigate expression trees as per https://stackoverflow.com/questions/9505189/dynamically-generate-linq-queries
    }
}
