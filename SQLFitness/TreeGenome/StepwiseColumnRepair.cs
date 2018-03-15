using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class StepwiseColumnRepair : Visitor
    {
        private bool _done;
        private Node _tree;

        public StepwiseColumnRepair(Node tree, int componentRestrictions = 3)
        {
            this.Visit(tree);
        }

        protected override void Visit(BinaryNode visitedNode)
        {


        }

        private int _countOccurrences(PredicateNode p1, List<PredicateNode> nodelist)
        {
            return default;
        }

        protected override void Visit(PredicateNode visitedNode)
        {
           
            
        }

        public Node GetTree()
        {
            if (!_done) throw new InvalidOperationException("Tree not finished walking");
            return _tree ?? throw new InvalidOperationException("Tree hasn't been visited yet");
        }

        
    }
}
