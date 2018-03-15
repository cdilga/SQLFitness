using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public abstract class Visitor
    {
        /// <summary>
        /// Depth First Search tree traversal
        /// </summary>
        /// <param name="visitedNode">Takes a node which it will visit</param>
        virtual protected void Visit(BinaryNode visitedNode)
        {
            Visit(visitedNode.Left);
            Visit(visitedNode.Right);
        }
        virtual protected void Visit(PredicateNode visitedNode)
        {

        }
        virtual protected void Visit(Node visitedNode)
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
