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
    public class MutateWalkerTests
    {
        private PredicateNode _replacementNode;
        private BinaryNode _binaryNode;
        private PredicateNode _position2Node;
        private PredicateNode _endPredicateNode;
        private BinaryNode _baseNode;

        private PredicateNode predicateBuilder(string col, string cell) => new PredicateNode(new List<string> { col }, x => new List<object> { cell });

        [SetUp]
        public void MutateWalkerSetup()
        {
            _endPredicateNode = predicateBuilder("ReplacedMeNode", "ReplaceMeCell");
            _position2Node = predicateBuilder("Position2Node", "Position2Cell");
            _binaryNode = new BinaryNode(_endPredicateNode, _endPredicateNode, BinaryNodeType.AND);
            _baseNode = new BinaryNode(_position2Node, _binaryNode, BinaryNodeType.OR);

            _replacementNode = predicateBuilder("Replaced", "ReplacedColumn");
        }

        [Test]
        public void MutateBinaryPos1()
        {
            var mutator = new MutateWalker(_baseNode, 1);
            Assert.AreNotEqual(_baseNode, _position2Node);
        }

        [Test]
        public void MutateWalkerTest()
        {
            var mutator = new MutateWalker(_baseNode, 2);
            Assert.AreNotEqual(_position2Node, ((BinaryNode)mutator.GetTree()).Left);
        }

        [Test]
        public void MutateLastPositionPredicate()
        {
            var leftPredicate = predicateBuilder("left", "left cell");
            var rightPredicate = predicateBuilder("right", "right cell");
            var base3Node = new BinaryNode(leftPredicate, rightPredicate);
            var mutator = new MutateWalker(base3Node, mutatePoint: base3Node.BranchSize);
            Assert.AreNotEqual(_endPredicateNode, ((BinaryNode)mutator.GetTree()).Right);
        }

        [TestCase(BinaryNodeType.AND)]
        [TestCase(BinaryNodeType.OR)]
        public void MutateBinaryNodeType(BinaryNodeType initialType)
        {
            var leftPredicate = predicateBuilder("left", "left cell");
            var rightPredicate = predicateBuilder("right", "right cell");
            var base3Node = new BinaryNode(leftPredicate, rightPredicate, initialType);
            var mutator = new MutateWalker(base3Node, 1);
            Assert.AreEqual(initialType, ((BinaryNode)mutator.GetTree()).NodeType);
        }
    }
}