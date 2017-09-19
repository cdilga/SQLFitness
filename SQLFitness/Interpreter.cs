using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SQLFitness
{
    class Interpreter
    {
        private Individual _sqlIndividual;
        private readonly string _tableName;

        //This object is required to communicate with the DBAccess to get a subset of the data back
        //This object will know about SQL Syntax. An interpreter is essentially a kind of parser, so It'll be passed in an individual which it will then convert into something usable
        public Interpreter( string tableName)
        {
            
            _tableName = tableName;
        }

        public string Parse(Individual sqlIndividual)
        {
            _sqlIndividual = sqlIndividual;
            return _generateSql(sqlIndividual.Genome);
        }

        private string _concatenate(List<IChromosome> chromosomes)
        {
            return String.Join(" ", chromosomes);
        }

        private List<IChromosome> selectType(List<IChromosome> chromosomes, Type type)
        {
            var returnList = new List<IChromosome>();
            foreach (var chromosome in chromosomes)
            {
                if (chromosome.GetType() == type)
                {
                    returnList.Add(chromosome);
                }
            }
            return returnList;
        }
        private string _generateSql(List<IChromosome> chromosomes)
        {
            List<IChromosome> _projections = selectType(chromosomes, typeof(Projection));
            List<IChromosome> _selections = selectType(chromosomes, typeof(Selection));

            var catenatedProjections = String.Join(" ", _projections);
            var selectComponent = catenatedProjections.Any() ? catenatedProjections : "*";
            var query = $"SELECT { selectComponent } FROM { _tableName }";

            var catenatedSelections = String.Join(" AND ", _selections);
            if (catenatedSelections.Any())
            {
                query += $" WHERE {catenatedSelections}";
            }
            //var catenatedGrouping = String.Join(" ", _groups);
            // if (catenatedSelections.Any())
            //{
            //    query += $"GROUP BY {catenatedGrouping}";
            //}
            return query;
        }
    }
}
