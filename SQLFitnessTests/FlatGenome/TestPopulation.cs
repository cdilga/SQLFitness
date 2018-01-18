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
        //private class fit: IFitness
        //{
        //    public double Evaluate(StubIndividual individual)
        //    {
        //        index++;
        //        return _fitness[index];
        //    }
        //}

        private static StubIndividual individualFactory(double fitness, List<string> validColumns, Func<string, List<object>> validDataGetter)
        {
            var individual1 = new FlatIndividual(validColumns, validDataGetter);
            individual1.Fitness = new Fitness(fitness);
            return individual1;
        }
        private Population _population;
        [SetUp]
        public void SetupPopulation()
        {
            var validColumns = new List<string> { "Column 1", "Column 2" };
            Func<string, List<object>> validDataGetter = (string x) => new List<object> { "Data 1", "Data 2" };
            _population = new Population();
            foreach (var x in _fitness)
            {
                _population.Add(new FlatIndividual(validColumns, validDataGetter));
            }
        }
        [Test]
        public void TestSort()
        {
            // TODO: Add your test code here
            _population = new Population();
            var individual1 = new TestIndividual(10.0);
            var individual2 = new TestIndividual(4.1);
            var individual3 = new TestIndividual(4.2);
            _population.Add(individual3);
            _population.Add(individual2);
            _population.Add(individual1);
            _population.Sort();
            Assert.AreEqual(individual1, _population[0]);
            Assert.AreEqual(individual3, _population[1]);
            Assert.AreEqual(individual2, _population[2]);

        }

        private class TestIndividual : FlatIndividual
        {
            public TestIndividual(double fitness) : base(new List<string> { "" }, (x) => new List<object> { "" })
            {
                this.Fitness = new Fitness(fitness);
            }
        }
    }
}
