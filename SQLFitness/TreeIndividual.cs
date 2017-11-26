using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class TreeIndividual : StubIndividual
    {
        private Projection[] _projectionGenome;
        private Node _selectionTree;
        private int _branchSize { get => _selectionTree.BranchSize; }
        private readonly List<String> _validColumns;
        private readonly Func<string, List<object>> _validDataGetter;

        public TreeIndividual(List<String> validColumns, Func<string, List<object>> validDataGetter)
        {
            
            _projectionGenome = new Projection[Math.Min(Utility.MaxTreeChromosomeProjectionSize, validColumns.Count)];
            //Create new tree
            var treeBuilder = new RandomBuilder(validColumns, validDataGetter, Utility.TreeChromosomeBranchSize);
            var thing = new ComponentRestrictionRepair(3);
            thing.Visit(treeBuilder.Build());
            _selectionTree = thing.GetTree();

            //Create new predicates
            for (var i = 0; i < _projectionGenome.Length; i++)
            {
                Projection temp;
                do
                {
                    temp = new Projection(validColumns);
                } while (_projectionGenome.Any(x => x?.Field == temp.Field && x != null));
                //When we have one that isn't in the array add it to the projectiongenome and continue iterating
                _projectionGenome[i] = temp;
            }

            _validColumns = validColumns ?? throw new ArgumentNullException(nameof(validColumns));
            _validDataGetter = validDataGetter ?? throw new ArgumentNullException(nameof(validDataGetter));
#if DEBUG
            Utility.TestDuplicates(_projectionGenome.ToList());
#endif
        }

        private TreeIndividual(List<String> validColumns, Func<string, List<object>> validDataGetter, Node selectionTree, Projection[] projectionGenome)
        {
            _validColumns = validColumns ?? throw new ArgumentNullException(nameof(validColumns));
            _validDataGetter = validDataGetter ?? throw new ArgumentNullException(nameof(validDataGetter));
            _selectionTree = selectionTree ?? throw new ArgumentNullException(nameof(selectionTree));
            _projectionGenome = projectionGenome ?? throw new ArgumentNullException(nameof(projectionGenome));
#if DEBUG
            Utility.TestDuplicates(_projectionGenome.ToList());
#endif
        }

        public override void Mutate()
        {
            if (Utility.GetRandomNum(2) == 1)
            {
                var mutator = new MutateWalker(Utility.GetRandomNum(_selectionTree.BranchSize + 1));
                mutator.Visit(_selectionTree);
                _selectionTree = mutator.GetTree();
            }
            else
            {
                _projectionGenome[Utility.GetRandomNum(_projectionGenome.Length)] = new Projection(_validColumns.Distinct().ToList()); 
            }
            this.Fitness = null;
            _projectionGenome = _projectionGenome.DistinctChromosomes();
            var thing = new ComponentRestrictionRepair(3);
            thing.Visit(_selectionTree);
            _selectionTree = thing.GetTree();
        }
        public override string ToSql()
        {
            var sqlGenerator = new InterpretWalker();
            sqlGenerator.Visit(_selectionTree);

            var tempSelections = new List<string>();

            var _projections = _projectionGenome;
            var catenatedProjections = String.Join(", ", _projections.Select(x => $"`{x}`"));
            var selectComponent = catenatedProjections.Any() ? catenatedProjections : "*";
            var query = $"SELECT { selectComponent } FROM { Utility.TableName } {sqlGenerator.GetWhereClause()}";
            return query + ";\n";
        }

        protected override StubIndividual CrossWithSpouse(StubIndividual spouse)
        {
            //Randomly select a point to cross on the tree
            var tempSpouse = (TreeIndividual)spouse;
            var crossCutter = new CrossWalker(Utility.GetRandomNum(tempSpouse._branchSize + 1));
            crossCutter.Visit(tempSpouse._selectionTree);
            var addBranchAt = new AddBranchWalker(Utility.GetRandomNum(_selectionTree.BranchSize + 1), crossCutter.GetGenomeSubTree());
            addBranchAt.Visit(_selectionTree);
            //Randomly select a point to cross on the projections
            var newChromosome = new List<Projection>();
            for (var i = 0; i < Utility.GetRandomNum(_projectionGenome.Length); i++)
            {
                newChromosome.Add(_projectionGenome[i]);
            }
            for (var i = Utility.GetRandomNum(tempSpouse._projectionGenome.Length); i < tempSpouse._projectionGenome.Length; i++)
            {
                newChromosome.Add(tempSpouse._projectionGenome[i]);
            }
            var distinct = newChromosome.DistinctChromosomes().ToArray();
            var thing = new ComponentRestrictionRepair(3);
            thing.Visit(addBranchAt.GetTree());
            return new TreeIndividual(_validColumns, _validDataGetter, thing.GetTree(), distinct);
        }
    }
}
