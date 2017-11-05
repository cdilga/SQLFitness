using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    [TestFixture]
    public class TestChromosome
    {
        private FlatIndividual _individualBuilder(
            string col,
            string cell) => new FlatIndividual(new List<string> { "Column1" }, x => new List<object> { "Cell1" });
        FlatIndividual _individual;

        [SetUp]
        public void SetupIndividual() => _individual = _individualBuilder("Column1", "Cell1");

        [Test]
        public void IndividualCrosses()
        {
            var _individual2 = _individualBuilder("Column2", "Cell2");
            var sql = ((FlatIndividual)_individual.Cross(_individual2)).ToSql();
            Assert.IsTrue(sql.Contains("Column1") || sql.Contains("Column2"));
        }
    }
}
