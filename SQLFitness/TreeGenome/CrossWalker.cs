using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome
{
    public class CrossWalker : Visitor
    {
        private int _position = 0;
        private Node _crossNode = null;
        private readonly int _cutPoint;

        public CrossWalker(int cutPoint)
        {
            _cutPoint = cutPoint;
        }

        public override void Visit(BinaryNode visitedNode)
        {
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

        public override void Visit(PredicateNode visitedNode)
        {
            _position += 1;
            if (_position == _cutPoint)
            {
                _crossNode = visitedNode;
            }
        }

        public Node GetGenomeSubTree() => _crossNode ?? throw new InvalidOperationException("Have not got the cross-node yet");
    }
}
