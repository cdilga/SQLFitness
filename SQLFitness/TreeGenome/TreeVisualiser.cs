using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class TreeVisualiser: Visitor
    {
        //Depth is going to be increased each time we go down, and 
        //then reversed on the way back up
        private int _depth;
        private int _position;

        private string _result;
        private List<string> _labelList;

        private bool _first;
        //We're going to need a string buider that has a number of rows that is equal to the number of nodes on the tree
        private StringBuilder _treeAsString;
        private readonly bool _positionPrefix;

        public TreeVisualiser(bool positionPrefix = false)
        {

            _treeAsString = new StringBuilder();
            _positionPrefix = positionPrefix;
        }

        public void Visit(BinaryNode visitedNode)
        {
            if (!_first)
            {
                _labelList = new List<string>(visitedNode.BranchSize);
                _first = true;
            }
            _depth++;

            var positionPrefix = "";
            if (_positionPrefix)
            {
                positionPrefix = " " + _position.ToString();
            }
            var depthDelimter = "|   ";
            var depthStringBuilder = new StringBuilder();
            
            for (var i = 0; i < _depth; i++)
            {
                depthStringBuilder.Append(depthDelimter);
            }

            _labelList[_position] = $"{depthStringBuilder}{positionPrefix} {visitedNode.NodeType}";
            _position++;

            Visit(visitedNode.Left);
            Visit(visitedNode.Right);

            _depth--;

        }
        public void Visit(PredicateNode visitedNode)
        {
            _depth++;
            var positionPrefix = "";
            if (_positionPrefix)
            {
                positionPrefix = " " + _position.ToString();
            }
            var depthDelimter = "|   ";
            var depthStringBuilder = new StringBuilder();

            for (var i = 0; i < _depth; i++)
            {
                depthStringBuilder.Append(depthDelimter);
            }

            _labelList[_position] = $"{depthStringBuilder}{positionPrefix} {visitedNode.Left}{visitedNode.Condition}{visitedNode.Right}";
            _position++;

            _depth--;
        }

        public void print()
        {
            Console.WriteLine(result());
        }

        public string result()
        {
            foreach (var item in _labelList)
            {
                _treeAsString.AppendLine(item);
            }
            
            return _treeAsString.ToString();
        }
    }
}
