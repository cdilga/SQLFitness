using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class TerminalFitness : IFitness
    {
        public double Evaluate(Individual individual)
        {
            Console.WriteLine(individual.Genome.Count);
            return 1 / individual.Genome.Count;
        }

        public TerminalFitness()
        {

        }
    }
}
