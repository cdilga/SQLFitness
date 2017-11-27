using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SQLFitness
{
    public class PerformanceTestFitness : IFitness
    {
        public double[] Evaluate(StubIndividual individual)
        {
            double[] output = new double[2];
            output[0] = 0.0;
            output[1] = 0.0;
            return output;
        }
    }
}
