using NUnit.Framework;
using SQLFitness.TreeGenome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome.Tests
{
    [TestFixture()]
    public class RandomBuilderTests
    {
        private RandomBuilder _builder;
        private RandomBuilder buildFactory(int size) => new RandomBuilder(new List<string> { "Column1" }, x => new List<object> { "Cell1" }, size);
        [Test]
        public void RandomBuilderPredicateOnly()
        {
            _builder = buildFactory(1);
            //If it can cast the base node to a PredicateNode, we've succeeded
            var temp = (PredicateNode)_builder.Build();
            Assert.Pass();
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(101)]
        public void BuildTestSizes(int size)
        {
            var temp = buildFactory(size).Build();
            Assert.AreEqual(size, temp.BranchSize);
        }
    }
}