using MindFusion.Diagramming;
using MindFusion.Diagramming.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeDebugVisualizer
{
    public partial class TreeView : Form
    {

        public TreeView()
        {
            InitializeComponent();
        }

        private IVisualizableNode _rootNode;
        public IVisualizableNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; updateDiagram(); }
        }

        private void updateDiagram()
        {
            var rootNode = generateShapeNode(RootNode);
            var bounds = rootNode.Bounds;
            bounds.X += 40;
            bounds.Y += 10;
            rootNode.Bounds = bounds;
            rearrange(rootNode);
        }

        TreeLayout treeLayout;
        private void rearrange(ShapeNode rootNode)
        {
            if (treeLayout == null)
            {
                treeLayout = new TreeLayout(rootNode,
                    TreeLayoutType.Centered,
                    false,
                    TreeLayoutLinkType.Rounded,
                    TreeLayoutDirections.TopToBottom,
                    10, 5, true, new SizeF(10, 10));
            }

            treeLayout.Arrange(treeDiagram);
        }

        private ShapeNode generateShapeNode(IVisualizableNode visNode)
        {
            var node = new ShapeNode();
            node.Bounds = new RectangleF(0, 0, 18, 10);
            treeDiagram.Nodes.Add(node);
            styleNode(node);
            node.Text = visNode.NodeText;
            if (visNode.ChildNodes != null)
            {
                foreach (var child in visNode.ChildNodes)
                {
                    generateShapeNode(child, node);
                }
            }
            VisNodeTable.Add(node, visNode);
            return node;
        }

        private static ConditionalWeakTable<ShapeNode, IVisualizableNode> VisNodeTable = new ConditionalWeakTable<ShapeNode, IVisualizableNode>();

        private ShapeNode generateShapeNode(IVisualizableNode visNode, ShapeNode parent)
        {
            var node = generateShapeNode(visNode);
            node.AttachTo(parent, AttachToNode.BottomCenter);
            var link = new DiagramLink(treeDiagram, parent, node);
            styleLink(link);
            treeDiagram.Links.Add(link);
            return node;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (RootNode == null)
            {
                var rootNode = new ShapeNode(treeDiagram);
                rootNode.Bounds = new RectangleF(40, 15, 18, 10);
                styleNode(rootNode);
                rootNode.Text = $"No {nameof(RootNode)}!";

                treeDiagram.Nodes.Add(rootNode);
            }
        }

        private void styleNode(ShapeNode node)
        {
            node.Brush = new MindFusion.Drawing.SolidBrush(Color.White);
            node.Pen = new MindFusion.Drawing.Pen(Color.FromArgb(68, 140, 255), 0.9f);
            node.TextFormat.Alignment = StringAlignment.Center;
            node.TextFormat.LineAlignment = StringAlignment.Center;
            node.Font = new Font("Segoe UI", 12, FontStyle.Bold, GraphicsUnit.Point, 0);
            node.TextBrush = new MindFusion.Drawing.SolidBrush(Color.Blue);
        }

        private void styleLink(DiagramLink link)
        {
            link.Brush = new MindFusion.Drawing.SolidBrush(Color.Blue);
            link.HeadShape = ArrowHeads.Triangle;
            link.HeadShapeSize = 2.5f;
        }

        private void diagramView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.C)
            {
                var shapeNode = treeDiagram.ActiveItem as ShapeNode;
                if(shapeNode == null)
                {
                    return;
                }
                if(VisNodeTable.TryGetValue(shapeNode, out var visNode))
                {
                    //TODO Remove the reference to the debugging of the debugger visualiser from Test
                    var XMindString = new XMindVisualiser(visNode).GetTree();
                    Clipboard.SetText(XMindString);
                }
            }
        }
    }
}
