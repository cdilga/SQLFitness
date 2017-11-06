using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class AddBranchWalker : Visitor
    {
        private readonly int _cutPoint;
        private int _position = 0;
        private Node _addTree;
        private Node _tree;
        private Node _cutNode;
        private bool _done = false;
        private BinaryNode _replaceMeNode;
        private BinaryNode _replaceWithNode;

        /// <summary>
        /// Will create a new tree from the visited nodes, with <paramref name="subtree"/>
        /// being added at the <paramref name="cutPoint"/>.
        /// </summary>
        /// <param name="cutPoint">Point at which the <paramref name="subtree"/> is added</param>
        /// <param name="subtree">Tree added to the walked tree with <see cref="Visit(BinaryNode)"/></param>
        public AddBranchWalker(int cutPoint, Node subtree)
        {
            _cutPoint = cutPoint;
            _addTree = subtree ?? throw new ArgumentNullException(nameof(subtree));
            if (_cutPoint <= 1)
            {
                _done = true;
                _tree = _addTree;
            }
        }

        private BinaryNode _nodeDuplicator(BinaryNode oldNode) => new BinaryNode(oldNode.Left, oldNode.Right, oldNode.NodeType);
        private PredicateNode _nodeDuplicator(PredicateNode oldNode) => new PredicateNode(new List<string> { oldNode.Left }, x => new List<object> { oldNode.Right }, oldNode.Condition);


        public override void Visit(BinaryNode visitedNode)
        {
            _position++;

            //If it hits the cut point...
            if (_position == _cutPoint)
            {
                _cutNode = visitedNode;
                _done = true;
            }
            else
            {
                if (_cutNode == null)
                {
                    Visit(visitedNode.Left);
                }
                if (_cutNode == null)
                {
                    Visit(visitedNode.Right);
                }
            }

            //If the child of the node happens to be the _cutNode node...
            //Then you will create a new node here, with the existing opposite chain.
            //BinaryNode node;
            if (visitedNode.Left == _cutNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = new BinaryNode(_addTree, visitedNode.Right, visitedNode.NodeType);
            }
            else if (visitedNode.Right == _cutNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = new BinaryNode(visitedNode.Left, _addTree, visitedNode.NodeType);
            }
            else if (visitedNode.Left == _replaceMeNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = new BinaryNode(_replaceWithNode, visitedNode.Right, visitedNode.NodeType);
            }
            else if (visitedNode.Right == _replaceMeNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = new BinaryNode(visitedNode.Left, _replaceWithNode, visitedNode.NodeType);
            }
            //Now what happens when we reach the top?
            //When we have reached the top we will have the condition that a node has been replaced at least once on the left or right (considering this is already a BinaryNode)
            //So, the last iteration, and only the last iteration will need to set the _tree to the new node. This can be done each time
            //May be a case here where we can get an access before the end of the copying. Even if this is not the case,
            //we could do without the _tree, and just return the replacedwith node.
            _tree = _replaceWithNode != null ? _replaceWithNode: _tree;
        }

        public override void Visit(PredicateNode visitedNode)
        {
            _position++;
            if (_position == _cutPoint)
            {
                _cutNode = visitedNode;
                _done = true;
            }
        }

        public Node GetTree()
        {
            if (!_done) throw new InvalidOperationException("Tree not finished walking");
            return _tree ?? throw new InvalidOperationException("Tree hasn't been visited yet");
        }
    }
}
