using NUnit.Framework;
using SQLFitness.TreeGenome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome.Tests
{
    
    [TestFixture]
    public class AddBranchBuilderTests
    {
        private PredicateNode _replacementNode;
        private BinaryNode _replaceBinaryNode;
        private PredicateNode _keepPredicateNode;
        private PredicateNode _replacePredicateNode;
        private BinaryNode _baseNode;

        [SetUp]
        public void AddBranchBuilderSetup()
        {
            _replacePredicateNode = predicateBuilder("ReplacedMeNode", "ReplaceMeCell");
            _keepPredicateNode = predicateBuilder("KeepMeNode", "KeepMeCell");
            _replaceBinaryNode = new BinaryNode(_replacePredicateNode, _replacePredicateNode, BinaryNodeType.AND);
            _baseNode = new BinaryNode(_replaceBinaryNode, _keepPredicateNode, BinaryNodeType.OR);

            _replacementNode = predicateBuilder("Replaced", "ReplacedColumn");
        }
        private PredicateNode predicateBuilder(string col, string cell) => new PredicateNode(new List<string> { col }, x => new List<object> { cell });
        [Test]
        public void AddBranchBuilderSinglePredicateNode()
        {
            var predicateNode = predicateBuilder("ReplacedMeNode", "ReplaceMeCell");
            var replacementNode = predicateBuilder("Replaced", "ReplacedCell");
            var branchAdder = new AddBranchBuilder(1, replacementNode);
            branchAdder.Visit(predicateNode);
            Assert.AreEqual(replacementNode, branchAdder.GetTree());
        }

        [Test]
        public void AddBranchBuilderOverLargeTree()
        {
            //Should return the replacement node as this is being cut at position 1 again
            var predicateNode = predicateBuilder("ReplacedMeNode", "ReplaceMeCell");
            var predicateNode2 = predicateBuilder("ReplaceMeNode", "ReplaceMeCell");
            var binaryNode = new BinaryNode(predicateNode, predicateNode2, BinaryNodeType.AND);
            var binaryNode2 = new BinaryNode(binaryNode, predicateNode2, BinaryNodeType.OR);

            var replacementNode = predicateBuilder("Replaced", "ReplacedColumn");

            var branchAdder = new AddBranchBuilder(1, replacementNode);
            branchAdder.Visit(binaryNode2);
            Assert.AreEqual(replacementNode, branchAdder.GetTree());
        }

        [Test]
        public void AddBranchBuilderPosition2() {
            var branchAdder = new AddBranchBuilder(2, _replacementNode);
            branchAdder.Visit(_baseNode);

            var resultNode = branchAdder.GetTree();
            Assert.AreEqual(_replacementNode, ((BinaryNode)resultNode).Left);
            Assert.AreEqual(_keepPredicateNode, ((BinaryNode)resultNode).Right);
            Assert.AreEqual(_baseNode.NodeType, ((BinaryNode)resultNode).NodeType);
        }
        [Test]
        public void AddBranchBuilderAtLeaf()
        {
            var branchAdder = new AddBranchBuilder(3, _replacementNode);
            branchAdder.Visit(_baseNode);

            var resultNode = branchAdder.GetTree();
            Assert.AreEqual(_replacementNode, ((BinaryNode)((BinaryNode)resultNode).Left).Left);
            Assert.AreEqual(_replacePredicateNode, ((BinaryNode)((BinaryNode)resultNode).Left).Right);
            Assert.AreEqual(_baseNode.NodeType, ((BinaryNode)resultNode).NodeType);
            Assert.AreEqual(_baseNode.Right, ((BinaryNode)resultNode).Right);
        }

        [Test]
        public void AddBranchBuilderFullTreeLast()
        {
            var branchAdder = new AddBranchBuilder(_baseNode.BranchSize, _replaceBinaryNode);
            branchAdder.Visit(_baseNode);

            var resultNode = branchAdder.GetTree();
            Assert.AreEqual(_replacementNode, ((BinaryNode)resultNode).Right);
            Assert.AreEqual(_baseNode.NodeType, ((BinaryNode)resultNode).NodeType);
            Assert.AreEqual(_baseNode.Left, ((BinaryNode)resultNode).Left);
        }

        /*
        [Test()]
        public void AddBranchBuilderTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void VisitTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void VisitTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetTreeTest()
        {
            Assert.Fail();
        }*/
    }
}