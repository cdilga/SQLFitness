using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    /// <summary>
    /// Implement a fitness function to be performed on an individual
    /// </summary>
    public interface IFitness
    {
        double Evaluate(StubIndividual individual);
    }
}
