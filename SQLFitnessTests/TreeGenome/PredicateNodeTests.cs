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
    public class PredicateNodeTests
    {
        PredicateNode _predicate;
        [SetUp]
        public void SetupPredicate()
        {

        }

        [Test()]
        public void PredicateNodeTest()
        {
            _predicate = new PredicateNode(new List<string> { "Col1", "Col2" }, str => new List<object> { "Cell1", "Cell2" });
        }

        [Test]
        public void PredicateMutate()
        {
            Assert.Fail();
        }
    }
}