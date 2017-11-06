using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class RandomBuilder
    {
        private readonly List<string> _validColumns;
        private readonly int _branchSize;
        //If the current build were to be capped off with all predicates, then
        //the size would be capsize
        private int _currCapSize = 1;
        private readonly Func<string, List<object>> _validDataGetter;

        public RandomBuilder(List<string> validColumns, Func<string, List<object>> validDataGetter, int branchSize)
        {
            if (branchSize % 2 != 1)
            {
                throw new ArgumentException($"Invalid branch size: {branchSize}. Must be odd");
            }

            _validColumns = validColumns ?? throw new ArgumentNullException(nameof(validColumns));
            _branchSize = branchSize;
            _validDataGetter = validDataGetter ?? throw new ArgumentNullException(nameof(validDataGetter));
        }

        public Node Build()
        {
            if (_branchSize == 1)
            {
                return new PredicateNode(_validColumns, _validDataGetter);
            }
            else if (_currCapSize < _branchSize - 5)
            {
                
                switch (Utility.GetRandomNum(4))
                {
                    case 0:
                    case 1:
                        _currCapSize += 4;
                        return new BinaryNode(Build(), Build());
                    case 2:
                        _currCapSize += 2;
                        return new BinaryNode(new PredicateNode(_validColumns, _validDataGetter), Build());
                    case 3:
                        _currCapSize += 2;
                        return new BinaryNode(Build(), new PredicateNode(_validColumns, _validDataGetter));
                    default:
                        throw new Exception("random number failed to address cases");

                }
            }
            else if (_currCapSize < _branchSize - 3)
            {
                _currCapSize += 2;
                if (Utility.GetRandomNum(2) == 0)
                {
                    return new BinaryNode(Build(), new PredicateNode(_validColumns, _validDataGetter));
                }
                else
                {
                    return new BinaryNode(new PredicateNode(_validColumns, _validDataGetter), Build());
                }
            }
            else if(_currCapSize < _branchSize - 1)
            {
                return new BinaryNode(new PredicateNode(_validColumns, _validDataGetter), new PredicateNode(_validColumns, _validDataGetter));
            }
            else
            {
                return new BinaryNode(new PredicateNode(_validColumns, _validDataGetter), new PredicateNode(_validColumns, _validDataGetter));
            }
        }
    }
}
