using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class ComponentRestrictionRepair : Visitor
    {
        private readonly int _componentRestrictions;
        private int _position = 0;
        private Node _tree;
        private Node _mutateNode;
        private bool _done = false;
        private Node _replaceMeNode;
        private Node _replaceWithNode;
        private PredicateNode[] _nodeList;

        private Boolean _first = true;

        private Node _deleteMeNode;

        /// <summary>
        /// Will create a new tree mutating at point <paramref name="componentRestrictions"/>.
        /// </summary>
        /// <param name="componentRestrictions">Point at which the visited node tree is mutated</param>
        public ComponentRestrictionRepair(int componentRestrictions = 3)
        {
            _componentRestrictions = componentRestrictions;
        }

        private BinaryNode _nodeDuplicator(BinaryNode oldNode) => new BinaryNode(oldNode.Left, oldNode.Right, oldNode.NodeType);
        private PredicateNode _nodeDuplicator(PredicateNode oldNode) => new PredicateNode(new List<string> { oldNode.Left }, x => new List<object> { oldNode.Right }, oldNode.Condition);

        private Boolean _isSame(PredicateNode p1, PredicateNode p2)
        {

            throw new NotImplementedException();
        }

        public override void Visit(BinaryNode visitedNode)
        {
            if (_first)
            {
                //Almost like a construction step
                _first = false;
                _nodeList = new PredicateNode[visitedNode.BranchSize];
                _tree = visitedNode;
            }

            //Visit left
            Visit(visitedNode.Left);
            if (visitedNode.Left == _deleteMeNode)
            {
                _replaceMeNode = visitedNode;
                _replaceWithNode = visitedNode.Right;
            }

            //Visit right
            Visit(visitedNode.Right);
            if (visitedNode.Right == _deleteMeNode)
            {
                _replaceMeNode = visitedNode;
                //Could be a problem if the left node is being deleted as well....
                _replaceWithNode = visitedNode.Left;
            }

            _position++;
        }

        private int _countOccurrences(PredicateNode p1, PredicateNode[] nodelist)
        {
            var count = 0;
            foreach (var node in nodelist)
            {
                if (node.Left == p1.Left)
                {
                    count++;
                }
            }
            return count;
        }

        public override void Visit(PredicateNode visitedNode)
        {
            _position++;
            if (_countOccurrences(visitedNode, _nodeList) > _componentRestrictions)
            {
                _deleteMeNode = visitedNode;
                return;
            }
            
            _nodeList[_position - 1] = visitedNode;
            
        }

        public Node GetTree()
        {
            //if (!_done) throw new InvalidOperationException("Tree not finished walking");
            return _tree ?? throw new InvalidOperationException("Tree hasn't been visited yet");
        }
    }
}
