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
    public class InterpretWalkerTests
    {
        private InterpretWalker _interpreterWalker;
        private PredicateNode _node3;
        private PredicateNode _node2;
        
        [SetUp]
        public void InterpretWalkerSetup()
        {
            _node3 = new PredicateNode(new List<string> { "Column3" } , str => new List<object> { "Cell3" }, PredicateType.Equal);
            _node2 = new PredicateNode(new List<string> { "Column2" }, str => new List<object> { "Cell2" }, PredicateType.LessThanEqual);
            _interpreterWalker = new InterpretWalker();
        }
        [Test]
        public void InterpretToSql()
        {
            var node1 = new BinaryNode(_node2, _node3, BinaryNodeType.AND);
            _interpreterWalker.Visit(node1);
            var sql = _interpreterWalker.GetWhereClause();
            Assert.AreEqual("WHERE (`Column2` <= 'Cell2') AND (`Column3` = 'Cell3')", sql);
        }

        [Test()]
        public void InterpretWalkerTest()
        {
            var node1 = new BinaryNode(_node2, _node3, BinaryNodeType.AND);
            var node0 = new BinaryNode(node1, node1, BinaryNodeType.OR);
            _interpreterWalker.Visit(node0);
            var sql = _interpreterWalker.GetWhereClause();
            Assert.AreEqual("WHERE ( (`Column2` <= 'Cell2') AND (`Column3` = 'Cell3')) OR ( (`Column2` <= 'Cell2') AND (`Column3` = 'Cell3'))", sql);
        }
    }
}