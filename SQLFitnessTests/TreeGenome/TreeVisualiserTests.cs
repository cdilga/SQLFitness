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
    public class TreeVisualiserTests
    {
        private PredicateNode predicateBuilder(string col, string cell, PredicateType type) => new PredicateNode(new List<string> { col }, x => new List<object> { cell }, type);

        [Test()]
        public void VisitTest()
        {
            var testVis = new TreeVisualiser();

            var node = new BinaryNode(predicateBuilder("Col1", "Cell1", PredicateType.Equal), predicateBuilder("Col2", "Cell2", PredicateType.Equal));
            testVis.Visit(node);
            Console.WriteLine(testVis.result());
        }
    }
}