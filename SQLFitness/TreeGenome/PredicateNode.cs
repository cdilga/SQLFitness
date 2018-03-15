using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeDebugVisualizer;

namespace SQLFitness
{
    [Serializable]
    public class PredicateNode : Node
    {
        public override int BranchSize => 1;
        private List<string> ValidData { get; }
        private Func<string, List<object>> ValidDataGetter { get; }
        public PredicateType Condition { get; }
        private static readonly PredicateType[] _predicateTypes = (PredicateType[])Enum.GetValues(typeof(PredicateType));

        public string Left { get; }
        public object Right { get; }

        public override string NodeText => $"{Left} {Condition.ToSQL()} {Right}";

        public override IReadOnlyCollection<IVisualizableNode> ChildNodes => default;

        public PredicateNode (List<string> validData, Func<string, List<object>> validDataGetter)
        {
            this.ValidData = validData ?? throw new ArgumentNullException(nameof(validData));
            this.Left = validData.GetRandomValue();
            this.Right = validDataGetter(this.Left).GetRandomValue();
            this.ValidDataGetter = validDataGetter ?? throw new ArgumentNullException(nameof(validDataGetter));
            

            this.Condition = _predicateTypes.GetRandomValue();
        }

        public PredicateNode(List<string> validData, Func<string, List<object>> validDataGetter, PredicateType condition) : this (validData, validDataGetter)
        {
            this.Condition = condition;
        }

        public PredicateNode Mutate() => new PredicateNode(ValidData, ValidDataGetter);

        public PredicateNode Mutate(List<string> validData, Func<string, List<object>>validDataGetter) => new PredicateNode(validData, validDataGetter);
    }
}
