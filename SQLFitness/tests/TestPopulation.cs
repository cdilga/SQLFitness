using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    [TestFixture]
    public class TestPopulation
    {
        private static List<double> _fitness = new List<double> { 0.2, 1.45, 2.67, 2.66 };
        private static int index = 0;
        private class fit: IFitness
        {
            public double Evaluate(Individual individual)
            {
                index++;
                return _fitness[index];
            }
        }

        private static Individual individualFactory(double fitness, List<string> validColumns, Func<string, List<object>> validDataGetter)
        {
            var individual1 = new Individual(validColumns, validDataGetter);
            individual1.Fitness = new Fitness(fitness);
            return individual1;
        }
        private Population _population;
        [SetUp]
        public void SetupPopulation()
        {
            var validColumns = new List<string> { "Column 1", "Column 2" };
            Func<string, List<object>> validDataGetter = (string x) => new List<object> { "Data 1", "Data 2" };
            _population = new Population(new fit());
            foreach (var x in _fitness)
            {
                _population.Add(new Individual(validColumns, validDataGetter));
            }
        }
        [Test]
        public void TestSort()
        {
            // TODO: Add your test code here
            Assert.Fail("Not Implemented");
        }

        //Test that a fitness function is called

    }
}
