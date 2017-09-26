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
        private static Individual individualFactory(double fitness, List<string> validColumns, Func<string, List<object>> validDataGetter)
        {
            var individual1 = new Individual(validColumns, validDataGetter);
            individual1.Fitness = fitness;
            return individual1;
        }
        private Population _population;
        [SetUp]
        public void SetupPopulation()
        {
            var validColumns = new List<string> { "Column 1", "Column 2" };
            Func<string, List<object>> validDataGetter = (string x) => new List<object> { "Data 1", "Data 2" };
            _population = new Population();
            foreach (var fitness in new List<double> { 0.2, 1.45, 2.67, 2.66 })
            {
                _population.Add(individualFactory(fitness, validColumns, validDataGetter));
            }
        }
        [Test]
        public void TestSort()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
    }
}
