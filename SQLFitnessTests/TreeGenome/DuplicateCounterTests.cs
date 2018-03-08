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
    public class DuplicateCounterTests
    {
        private PredicateNode predicateBuilder(string col, string cell, PredicateType type) => new PredicateNode(new List<string> { col }, x => new List<object> { cell }, type);

        DuplicateCounter counter;
        [SetUp()]
        public void Setup()
        {
            counter = new DuplicateCounter();
        }
        [Test()]
        public void VisitTest()
        {
            var rightRight = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var left = new BinaryNode(predicateBuilder("Col1", "Cell1", PredicateType.LessThan), predicateBuilder("Col1", "Cell1", PredicateType.LessThan));
            var right = new BinaryNode(predicateBuilder("Col1", "Cell1", PredicateType.LessThan), rightRight);
            var testTree = new BinaryNode(left, right);
            var correctTree = new BinaryNode(left, rightRight);
            counter.Visit(testTree);
            Assert.AreEqual(new Dictionary<Node, int>(), counter.RemoveItemsList());
        }

        [Test()]
        public void VisitTest1()
        {
            Assert.Fail();
        }
    }
}