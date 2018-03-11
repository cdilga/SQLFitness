using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDebugVisualizer
{
    [DebuggerVisualizer(typeof(NodeTreeVisualizer))]
    [Serializable]
    public class DebuggableNode : IVisualizableNode
    {
        public DebuggableNode(string NodeText, IReadOnlyCollection<DebuggableNode> ChildNodes)
        {
            this.NodeText = NodeText;
            this.ChildNodes = ChildNodes;
        }

        public string NodeText { get; }

        public IReadOnlyCollection<IVisualizableNode> ChildNodes { get; }
    }

    public interface IVisualizableNode
    {
        string NodeText { get; }
        IReadOnlyCollection<IVisualizableNode> ChildNodes { get; }
    }

    public static class Ext
    {
        public static DebuggableNode ToDebuggableNode(this IVisualizableNode visNode)
            => visNode as DebuggableNode
            ?? new DebuggableNode(visNode.NodeText, visNode.ChildNodes?.Select(ToDebuggableNode).ToList());
    }
}