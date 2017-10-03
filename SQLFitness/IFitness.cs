using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    /// <summary>
    /// Implement a fitness function to be performed on an individual
    /// </summary>
    interface IFitness
    {
        double Evaluate(Individual individual);
    }
}
