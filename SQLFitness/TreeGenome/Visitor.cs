using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome
{
    abstract class Visitor
    {
        public Visitor()
        {

        }


        virtual public void Visit(BinaryNode visitedNode)
        {
            Visit(visitedNode.Left);
            Visit(visitedNode.Right);
        }
        virtual public void Visit(PredicateNode visitedNode)
        {

        }
        virtual public void Visit(Node visitedNode)
        {
            switch (visitedNode)
            { 
                case BinaryNode binaryNode:
                    Visit(binaryNode);
                    break;
                case PredicateNode predicateNode:
                    Visit(predicateNode);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
