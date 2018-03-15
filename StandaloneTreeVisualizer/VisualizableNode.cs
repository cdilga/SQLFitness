using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDebugVisualizer
{


    public interface IVisualizableNode
    {
        string NodeText { get; }
        IReadOnlyCollection<IVisualizableNode> ChildNodes { get; }
    }
}