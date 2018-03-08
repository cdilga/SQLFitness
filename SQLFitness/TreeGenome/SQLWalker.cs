using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class SQLWalker : Visitor
    {
        private readonly StringBuilder _sqlBuilder;

        public SQLWalker(Node node)
        {
            _sqlBuilder = new StringBuilder("WHERE");
            this.Visit(node);
        }

        protected override void Visit(PredicateNode visitedNode)
        {
            //get left and right and make a stringy lookup of the enum and put that in the middle
            _sqlBuilder.Append($"`{visitedNode.Left}` ");
            _sqlBuilder.Append(visitedNode.Condition.ToSQL()).Append(" ");
            _sqlBuilder.Append($"'{visitedNode.Right}'");
        }

        protected override void Visit(BinaryNode visitedNode)
        {
            _sqlBuilder.Append(" (");
            Visit(visitedNode.Left);
            _sqlBuilder.Append(")");
            _sqlBuilder.Append(" ").Append(visitedNode.NodeType.ToSql()).Append(" ");
            _sqlBuilder.Append("(");
            Visit(visitedNode.Right);
            _sqlBuilder.Append(")");
        }

        public string GetWhereClause() => _sqlBuilder?.ToString() ?? throw new InvalidOperationException("Have not visited a node yet");
    }
}
