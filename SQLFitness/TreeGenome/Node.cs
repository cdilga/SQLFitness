using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TreeDebugVisualizer;

namespace SQLFitness
{
    [DebuggerDisplay("NodeSQL: {NodeDebugView()}")]
    [Serializable]
    //[DebuggerVisualizer(typeof(NodeTreeVisualizer), )]
    public abstract class Node : IVisualizableNode
    {
        public abstract int BranchSize { get; }
        

        public string NodeDebugView()
        {
            var walker = new SQLWalker(this);
            return walker.GetWhereClause();
        }

        public string XMindTree => ToXMindTree();

        public abstract string NodeText { get; }

        public abstract IReadOnlyCollection<IVisualizableNode> ChildNodes { get; }

        public string ToXMindTree(string name = default)
        {
                var walker = new XMindVisualiser(this, name);
                return walker.GetTree();
        }

        public DebuggableNode DebugNodeView => this.ToDebuggableNode();
    }
}
