using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeDebugVisualizer;

namespace SQLFitness
{
    [Serializable]
    public class BinaryNode : Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public BinaryNodeType NodeType { get; }
        public override int BranchSize { get; }

        public override string NodeText => $"{this.NodeType} ({this.BranchSize})";

        public override IReadOnlyCollection<IVisualizableNode> ChildNodes => new List<Node>() { this.Left, this.Right };

        private static readonly BinaryNodeType[] _binaryNodeTypes = (BinaryNodeType[])Enum.GetValues(typeof(BinaryNodeType));

        public BinaryNode(Node left, Node right, BinaryNodeType nodeType) : this(left, right)
        {
            this.NodeType = nodeType;
        }

        public BinaryNode(Node left, Node right)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
            this.NodeType = _binaryNodeTypes.GetRandomValue();
            this.BranchSize = left.BranchSize + right.BranchSize + 1;

        }
    }
}
