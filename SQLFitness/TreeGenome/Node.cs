using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace SQLFitness
{
    [DebuggerDisplay("NodeSQL: {NodeDebugView()}")]
    public abstract class Node
    {
        public abstract int BranchSize { get; }
        

        public string NodeDebugView()
        {
            var walker = new SQLWalker(this);
            return walker.GetWhereClause();
        }

        public string XMindTree => ToXMindTree();

        public string ToXMindTree(string name = default)
        {
                var walker = new XMindVisualiser(this, name);
                return walker.GetTree();
        }
    }
}
