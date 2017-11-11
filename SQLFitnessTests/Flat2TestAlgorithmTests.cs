using NUnit.Framework;
using SQLFitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.Tests
{
    [TestFixture()]
    public class DistinctTests
    {
        Func<Projection> projectionFactory = () => new Projection( new List<string>() { "Duplicate" } );
        [Test()]
        public void ExtensionMethods()
        {
            var first = projectionFactory();
            var newChromosome = new List<Projection>() { first, projectionFactory(), projectionFactory(), projectionFactory() };
            var distinct = newChromosome.DistinctChromosomes();
            Assert.AreEqual(new List<Projection>() { first}, distinct);
        }
    }
}