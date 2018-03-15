using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class TreeDuplicateRemoval : Visitor<Node>
    {
        public static Node RemoveDuplicates(Node tree, int maxDuplication = 3)
            => new TreeDuplicateRemoval(tree, maxDuplication).Output;

        private readonly int maxDuplication;

        Dictionary<string, int> columnCounts = new Dictionary<string, int>();
        protected TreeDuplicateRemoval(Node tree, int maxDuplication = 3) : base(tree)
        {
            this.maxDuplication = maxDuplication;
        }

        protected override Node Visit(PredicateNode node)
        {
            var columnName = node.Left;
            columnCounts.TryGetValue(columnName, out int count);
            columnCounts[columnName] = ++count;
            if (count > maxDuplication)
            {
                return null;
            }
            else
            {
                return node;
            }
        }

        protected override Node Visit(BinaryNode node)
        {
            var left = Visit(node.Left);
            var right = Visit(node.Right);
            if (left != null && right != null)
            {
                if (left == node.Left && right == node.Right)
                {
                    return node; //maintain reference equality
                }
                else
                {
                    return new BinaryNode(left, right, node.NodeType);
                }
            }
            else
            {
                return left ?? right;
            }
        }
    }

    public abstract class Visitor<T>
    {
        private readonly Lazy<T> _Output;
        protected T Output => _Output.Value;
        protected Visitor(Node tree)
        {
            //use lazy instead of calling Visit here so that derived constructors
            //can do further initialisation before calling Visit
            _Output = new Lazy<T>(() => Visit(tree));
        }

        protected virtual T Visit(Node node)
        {
            switch (node)
            {
                case BinaryNode binNode: return Visit(binNode);
                case PredicateNode predNode: return Visit(predNode);
                default:
                    throw new InvalidOperationException($"Cannot visit node of type {node.GetType().Name}");
            }
        }

        protected abstract T Visit(BinaryNode node);
        protected abstract T Visit(PredicateNode node);
    }
} 