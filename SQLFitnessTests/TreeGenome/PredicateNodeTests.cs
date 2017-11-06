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
            _predicate = new PredicateNode(new List<string> { "Col1" }, str => new List<object> { "Cell1" });
        }

        [Test]
        public void PredicateMutate()
        {
            Assert.AreEqual(_predicate.Left, _predicate.Mutate().Left);
            Assert.AreEqual(_predicate.Right, _predicate.Mutate().Right);
            Assert.AreNotEqual(_predicate, _predicate.Mutate());
            Assert.AreNotEqual(_predicate, _predicate.Mutate(new List<string> { "Not" }, x => new List<object> { "As before" }));
        }
    }
}