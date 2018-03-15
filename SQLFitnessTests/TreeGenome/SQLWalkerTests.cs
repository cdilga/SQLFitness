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
    public class SQLWalkerTests
    {
        private PredicateNode _node3;
        private PredicateNode _node2;
        
        [SetUp]
        public void InterpretWalkerSetup()
        {
            _node3 = new PredicateNode(new List<string> { "Column3" } , str => new List<object> { "Cell3" }, PredicateType.Equal);
            _node2 = new PredicateNode(new List<string> { "Column2" }, str => new List<object> { "Cell2" }, PredicateType.LessThanEqual);
        }

        [Test]
        public void InterpretToSql()
        {
            var node1 = new BinaryNode(_node2, _node3, BinaryNodeType.AND);
            var sql = new SQLWalker(node1).GetWhereClause();
            Assert.AreEqual("WHERE (`Column2` <= 'Cell2') AND (`Column3` = 'Cell3')", sql);
        }

        [Test()]
        public void InterpretWalkerTest()
        {
            var node1 = new BinaryNode(_node2, _node3, BinaryNodeType.AND);
            var node0 = new BinaryNode(node1, node1, BinaryNodeType.OR);
            var sql = new SQLWalker(node0).GetWhereClause();
            Assert.AreEqual("WHERE ( (`Column2` <= 'Cell2') AND (`Column3` = 'Cell3')) OR ( (`Column2` <= 'Cell2') AND (`Column3` = 'Cell3'))", sql);
        }
    }
}