using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome
{
    public class DuplicateCounter : Visitor
    {
        private int _cutoff;

        private bool _first;

        //Dictionary mapping nodes to counts
        private Dictionary<Node, int> _duplicateCounter;
        public DuplicateCounter(int cutoff = 3)
        {
            _cutoff = cutoff;
            _duplicateCounter = new Dictionary<Node, int>();
            _first = true;
        }

        public override void Visit(BinaryNode visitedNode)
        {
            if (_first)
            {
                //
            }
            Visit(visitedNode.Left);
            Visit(visitedNode.Right);
        }

        public override void Visit(PredicateNode visitedNode)
        {
            if (_first)
            {
                //We're going to add to the duplicateCounter and see if the correct results are generated here
            }
            //if the key exists, increase by 1
            if (_duplicateCounter.ContainsKey(visitedNode))
            {
                _duplicateCounter[visitedNode]++;
            }
            else
            {
                _duplicateCounter[visitedNode] = 1;
            }

            //otherwise, initialise the key at 1

        }

        public Dictionary<Node, int> RemoveItemsList { get => _duplicateCounter; }

    }
}
