using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class CrossWalker : Visitor
    {
        private int _position = 0;
        private Node _crossNode = null;
        private readonly int _cutPoint;

        public CrossWalker(Node tree, int cutPoint)
        {
            _cutPoint = cutPoint;
            Visit(tree);
        }

        protected override void Visit(BinaryNode visitedNode)
        {
            if (visitedNode == null)
            {
                throw new ArgumentNullException(nameof(visitedNode));
            }

            if (_cutPoint == 0)
            {
                _crossNode = visitedNode;
                return;
            }
            _position += 1;

            //If it hits the cut point...
            if (_position == _cutPoint)
            {
                _crossNode = visitedNode;
            }
            else
            {
                Visit(visitedNode.Left);
                if (_crossNode == null)
                {
                    Visit(visitedNode.Right);
                }
            }

            //If the child of the node happens to be the cross node...
            //Then you

        }

        protected override void Visit(PredicateNode visitedNode)
        {
            if (visitedNode == null)
            {
                throw new ArgumentNullException(nameof(visitedNode));
            }
            if (_cutPoint == 0)
            {
                _crossNode = visitedNode;
                return;
            }
            _position += 1;
            if (_position == _cutPoint)
            {
                _crossNode = visitedNode;
            }
        }

        public Node GetGenomeSubTree() => _crossNode ?? throw new InvalidOperationException("Have not got the cross-node yet");
    }
}
