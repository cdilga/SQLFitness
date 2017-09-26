using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    interface ISelector
    {
        double Evaluate(Individual individual);
    }
}
