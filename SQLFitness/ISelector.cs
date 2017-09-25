using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    interface ISelector
    {
        double evaluate(Individual individual);
    }
}
