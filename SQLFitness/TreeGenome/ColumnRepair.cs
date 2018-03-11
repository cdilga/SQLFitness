using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class ColumnRepair : Visitor
    {
        private readonly int _componentRestrictions;
        private int _position;
        private Node _tree;
        private Node _replaceMeNode;
        //_replaceWithNode will always start as a predicate node, but as it propagates it will change to binarynode
        private Node _replaceWithNode;

        //The purpose of this data is to record the number of occurrences of each of the different column names.
        //We need a way of mapping strings (columns) to a count. If the number is over a set number, delete that node.
        private Dictionary<String, int> _columnCounter;

        private Node _deleteMeNode;

        /// <summary>
        /// Will create a new tree mutating at point <paramref name="componentRestrictions"/>.
        /// </summary>
        /// <param name="componentRestrictions">Point at which the visited node tree is mutated</param>
        public ColumnRepair(Node tree, int componentRestrictions = 3)
        {
            _componentRestrictions = componentRestrictions;
            _columnCounter = GenerateDictionary(tree);

            _tree = tree;
            this.Visit(tree);
        }

        private Dictionary<string, int> GenerateDictionary(Node tree)
        {
            switch(tree)
            {
                case BinaryNode node:
                    return new Dictionary<string, int>((int)(tree.BranchSize / 2) + 1);
                case PredicateNode node:
                default:
                    return new Dictionary<string, int>(1);
            }
        }

        private BinaryNode _nodeDuplicator(BinaryNode oldNode) => new BinaryNode(oldNode.Left, oldNode.Right, oldNode.NodeType);
        private PredicateNode _nodeDuplicator(PredicateNode oldNode) => new PredicateNode(new List<string> { oldNode.Left }, x => new List<object> { oldNode.Right }, oldNode.Condition);

        private Boolean _isSame(PredicateNode p1, PredicateNode p2)
        {

            throw new NotImplementedException();
        }

        protected override void Visit(BinaryNode visitedNode)
        {
            //Probably like a "replace me" left and right, because what if you overwrote the replace me twice in one visit?

            Node left = visitedNode.Left;
            Node right = visitedNode.Right;
            _position++;

            Visit(visitedNode.Left);
            if (visitedNode.Left == _deleteMeNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = visitedNode.Right;
            }
            else if (visitedNode.Left == _replaceMeNode)
            {
                _replaceWithNode = new BinaryNode(_replaceWithNode, visitedNode.Right, visitedNode.NodeType);
                //_replaceMeNode = visitedNode;
                //What is left
                //left = _replaceWithNode;
            }

            Visit(visitedNode.Right);
            if (visitedNode.Right == _deleteMeNode)
            {
                _replaceMeNode = visitedNode;
                //Could be a problem if the left node is being deleted as well....
                _replaceWithNode = visitedNode.Left;
            }
            else if (visitedNode.Right == _replaceMeNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = new BinaryNode(visitedNode.Left, _replaceWithNode, visitedNode.NodeType);
                right = _replaceWithNode;
            }

            _tree = _replaceWithNode ?? _tree;
        }

        private int _countOccurrences(PredicateNode p1, List<PredicateNode> nodelist)
        {
            var count = 0;
            foreach (var node in nodelist)
            {
                //The left of the predicate node is the column
                if (node.Left == p1.Left)
                {
                    count++;
                }
            }
            return count;
        }

        protected override void Visit(PredicateNode visitedNode)
        {
            _position++;
            if (_columnCounter.ContainsKey(visitedNode.Left))
            {
                if (_columnCounter[visitedNode.Left] >= _componentRestrictions)
                {
                    _deleteMeNode = visitedNode;
                    return;
                }
                _columnCounter[visitedNode.Left]++;
            }
            else
            {
                _columnCounter[visitedNode.Left] = 1;
            }
            
        }

        public Node GetTree()
        {
            //if (!_done) throw new InvalidOperationException("Tree not finished walking");
            return _tree ?? throw new InvalidOperationException("Tree hasn't been visited yet");
        }
    }
}
