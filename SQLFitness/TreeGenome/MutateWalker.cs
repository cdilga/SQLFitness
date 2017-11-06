using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class MutateWalker : Visitor
    {
        private readonly int _mutatePoint;
        private int _position = 0;
        private Node _addTree;
        private Node _tree;
        private Node _mutateNode;
        private bool _done = false;
        private Node _replaceMeNode;
        private Node _replaceWithNode;
        private bool _random;

        /// <summary>
        /// Will create a new tree mutating at point <paramref name="mutatePoint"/>.
        /// </summary>
        /// <param name="mutatePoint">Point at which the visited node tree is mutated</param>
        public MutateWalker(int mutatePoint)
        {
            _mutatePoint = mutatePoint;
        }

        private BinaryNode _nodeDuplicator(BinaryNode oldNode) => new BinaryNode(oldNode.Left, oldNode.Right, oldNode.NodeType);
        private PredicateNode _nodeDuplicator(PredicateNode oldNode) => new PredicateNode(new List<string> { oldNode.Left }, x => new List<object> { oldNode.Right }, oldNode.Condition);


        public override void Visit(BinaryNode visitedNode)
        {
            if (_mutatePoint <= 1)
            {
                _done = true;
                _tree = visitedNode;
                return;
            }
            _position++;

            //If it hits the cut point...
            if (_position == _mutatePoint)
            {
                _mutateNode = visitedNode;
                _replaceMeNode = visitedNode;
                _replaceWithNode = new BinaryNode(
                    visitedNode.Left,
                    visitedNode.Right,
                    visitedNode.NodeType == BinaryNodeType.AND ? BinaryNodeType.OR : BinaryNodeType.AND
                    );
                _done = true;
            }
            else
            {
                if (_mutateNode == null)
                {
                    Visit(visitedNode.Left);
                }
                if (_mutateNode == null)
                {
                    Visit(visitedNode.Right);
                }
            }

            //If the child of the node happens to be the node we need to now replace,
            //we should then replace it
            if (visitedNode.Left == _replaceMeNode)
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
            _tree = _replaceWithNode ?? _tree;
        }

        public override void Visit(PredicateNode visitedNode)
        {
            if (_mutatePoint <= 1)
            {
                _done = true;
                _tree = visitedNode;
                return;
            }
            _position++;
            if (_position == _mutatePoint)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = visitedNode.Mutate();
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
