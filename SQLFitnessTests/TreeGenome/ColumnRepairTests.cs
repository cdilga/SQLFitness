using NUnit.Framework;
using SQLFitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.Tests
{
    [TestFixture()]
    public class ColumnRepairTests
    {
        private PredicateNode predicateBuilder(string col, string cell, PredicateType type) => new PredicateNode(new List<string> { col }, x => new List<object> { cell }, type);

        //Must take a single node and return it fine
        [Test()]
        public void VisitSingleNode()
        {
            var tree = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var repair = new ColumnRepair(tree);
            Assert.AreEqual(tree, repair.GetTree());
        }

        //Must take 3 node tree all with the same columns and return it fine too
        [Test]
        public void Visit3NodeTree()
        {
            var tree = new BinaryNode(predicateBuilder("Col1", "Cell1", PredicateType.LessThan), predicateBuilder("Col1", "Cell1", PredicateType.LessThan));
            var repair = new ColumnRepair(tree);
            var returnedTree = repair.GetTree();
            Assert.AreEqual(tree, returnedTree);
        }

        //Must take 7 node tree with 4 all the same, reduce down to smaller tree of size 5
        [Test]
        public void Visit7NodeTree()
        {
            var rightRight = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var left = new BinaryNode(predicateBuilder("Col1", "Cell1", PredicateType.LessThan), predicateBuilder("Col1", "Cell1", PredicateType.LessThan), BinaryNodeType.AND);
            var right = new BinaryNode(predicateBuilder("Col1", "Cell1", PredicateType.LessThan), rightRight, BinaryNodeType.AND);
            var testTree = new BinaryNode(left, right, BinaryNodeType.AND);
            var correctTree = new BinaryNode(left, rightRight, BinaryNodeType.AND);
            var repair = new ColumnRepair(testTree);
            var returnedTree = repair.GetTree();
            
            //TODO override reference equality
            Assert.AreEqual(correctTree.Left, ((BinaryNode)returnedTree).Left);
            Assert.AreEqual(correctTree.BranchSize, returnedTree.BranchSize);
            Assert.AreEqual(correctTree.NodeType, ((BinaryNode)returnedTree).NodeType);

            Assert.AreEqual(((PredicateNode)correctTree.Right).Condition, ((PredicateNode)((BinaryNode)returnedTree).Right).Condition);
            Assert.AreEqual(((PredicateNode)correctTree.Right).Left, ((PredicateNode)((BinaryNode)returnedTree).Right).Left);
            Assert.AreEqual(((PredicateNode)correctTree.Right).Right, ((PredicateNode)((BinaryNode)returnedTree).Right).Right);
            Assert.AreEqual(((PredicateNode)correctTree.Right).BranchSize, ((PredicateNode)((BinaryNode)returnedTree).Right).BranchSize);
        }

        //Test two different recurring (i.e. greater than 3) columns
        [Test]
        public void VisitMultiNodeTree()
        {
            var right = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var node7 = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var node8 = predicateBuilder("Col2", "Cell2", PredicateType.Equal);
            var node6 = new BinaryNode(node7, node8, BinaryNodeType.AND);
            var node9 = predicateBuilder("Col2", "Cell2", PredicateType.Equal);
            var node5 = new BinaryNode(node6, node9);
            var node10 = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var node4 = new BinaryNode(node5, node10, BinaryNodeType.AND);
            var node11 = predicateBuilder("Col2", "Cell2", PredicateType.Equal);
            var node3 = new BinaryNode(node4, node11);
            var node13 = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var node14 = predicateBuilder("Col2", "Cell2", PredicateType.Equal);
            var node12 = new BinaryNode(node13, node14);
            var node2 = new BinaryNode(node3, node12);
            var node15 = predicateBuilder("Col1", "Cell1", PredicateType.LessThan);
            var node1 = new BinaryNode(node2, node15);

            //Node 1 will be the original tree
            var repair = new ColumnRepair(node1);


            var correctTree = new BinaryNode(node3, node13, BinaryNodeType.AND);
            var resultTree = repair.GetTree();



            //var correctTreeXMIND = correctTree.ToXMindTree(nameof(correctTree));
            //var resultTreeXMIND = resultTree.ToXMindTree(nameof(resultTree));
            //var node1XMIND = node1.ToXMindTree(nameof(node1));


            var XmindTest = new TestCaseXMindVisualiser(node1, correctTree, resultTree).CreateJoinedXMindGraph();

            Assert.AreEqual(correctTree.BranchSize, resultTree.BranchSize);
        }

       
    }

    internal class TestCaseXMindVisualiser
    {
        private readonly Dictionary<string, Node> _debuggingNodes;
        public TestCaseXMindVisualiser(Node testTree, Node expectedTree, Node actualTree, [CallerMemberName] string testCase = "")
        {
            _debuggingNodes = new Dictionary<string, Node>
            {
                {nameof(testTree).ToUpper(), testTree },
                {nameof(expectedTree).ToUpper(), expectedTree },
                {nameof(actualTree).ToUpper(), actualTree}
            };
            this.TestCase = testCase;
        }

        public string TestCase { get; }

        public string CreateJoinedXMindGraph()
        {
            var joinedTree = new StringBuilder(TestCase).AppendLine();

            foreach (var (nodeName, node) in _debuggingNodes)
            {
                var tree = node.ToXMindTree(nodeName);
                joinedTree.Append("\t").Append(tree.Trim('\n').Replace("\n", "\n\t"));
            }
            return joinedTree.ToString();
        }
    }

}