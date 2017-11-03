using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness.TreeGenome
{
    public class PredicateNode: Node
    {
        public override int BranchSize => 1;
        private List<string> ValidData { get; }
        private Func<string, List<object>> ValidDataGetter { get; }
        public PredicateType Condition { get; }
        private static readonly PredicateType[] _predicateTypes = (PredicateType[])Enum.GetValues(typeof(PredicateType));
        public string Left { get; }
        public object Right { get; }
        public enum PredicateType { LessThan, GreaterThan, Equal, NotEqual, GreaterThanEqual, LessThanEqual }
        public PredicateNode (List<string> validData, Func<string, List<object>> validDataGetter)
        {
            this.ValidData = validData ?? throw new ArgumentNullException(nameof(validData));
            this.Left = validData.GetRandomValue();
            this.Right = validDataGetter(this.Left).GetRandomValue();
            this.ValidDataGetter = validDataGetter ?? throw new ArgumentNullException(nameof(validDataGetter));
            

            this.Condition = _predicateTypes.GetRandomValue();
        }

        public PredicateNode Mutate => new PredicateNode(ValidData, ValidDataGetter);
        //mutate
    }
}
