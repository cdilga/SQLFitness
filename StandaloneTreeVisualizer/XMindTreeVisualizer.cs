using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDebugVisualizer
{
    public class XMindVisualiser
    {
        private readonly StringBuilder _xmindTree;
        int _depth = 0;

        public XMindVisualiser(IVisualizableNode tree, string topLevelName = "")
        {
            _xmindTree = new StringBuilder(topLevelName);
            if (string.IsNullOrWhiteSpace(topLevelName))
            {
                _depth--;
            }
            else
            {
                _xmindTree.AppendLine();
            }

            
            this.Visit(tree);
        }

        protected void Visit(IVisualizableNode visitedNode)
        {
            _depth++;
            _xmindTree.Append('\t', _depth).AppendLine(visitedNode.NodeText);
            if (visitedNode.ChildNodes != null)
            {
                foreach (var node in visitedNode.ChildNodes)
                {
                    Visit(node);
                }
            }
            _depth--;
        }

        public string GetTree() => _xmindTree.ToString();
    }

}
