using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class XMindVisualiser : Visitor
    {
        private readonly StringBuilder _xmindTree;
        int _depth = 0;

        public XMindVisualiser(Node tree, string topLevelName = "")
        {
            if (string.IsNullOrWhiteSpace(topLevelName))
            {
                _depth--;
            }

            _xmindTree = new StringBuilder(topLevelName).AppendLine();
            this.Visit(tree);
        }

        protected override void Visit(PredicateNode visitedNode)
        {
            //get left and right and make a stringy lookup of the enum and put that in the middle
            //_xmindTree.Append('\t', _depth+1).AppendLine(visitedNode.Condition.ToString());
            //_xmindTree.Append('\t', _depth + 2).AppendLine(visitedNode.Left);
            //_xmindTree.Append('\t', _depth + 2).AppendLine(visitedNode.Right.ToString());
            _xmindTree.Append('\t', _depth + 1).Append(visitedNode.Left).Append(visitedNode.Condition.ToSQL()).AppendLine(visitedNode.Right.ToString());

        }

        protected override void Visit(BinaryNode visitedNode)
        {
            _depth++;
            _xmindTree.Append('\t', _depth).AppendLine(visitedNode.NodeType.ToString());
            Visit(visitedNode.Left);
            Visit(visitedNode.Right);
            _depth--;
        }

        public string GetTree() => _xmindTree?.ToString() ?? throw new InvalidOperationException("Have not visited a node yet");
    }
}
