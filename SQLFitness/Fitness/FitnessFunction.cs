using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    class FitnessFunction : IFitness
    {


        public Dictionary<string, string> KeywordsTags { get; }
        public Dictionary<string, float[]> Word2Vec { get; }

        public FitnessFunction(Dictionary<string, string> keywordsTags, Dictionary<string, float[]> word2vec)
        {
            KeywordsTags = keywordsTags;
            Word2Vec = word2vec;
        }


        private List<string> getCols()
        {
            var terms = new List<string>();
            //for (Op terminal: program.getTerminals())
            //{
            //    terms.add(terminal.toString().split("[<>]|(!=)|(==)|(<>)")[0]);
            //}
            return terms;
        }

        public double getRowsRelevance()
        {
            //                DescriptiveStatistics rowRelevance = new DescriptiveStatistics();
            //        if (result == null) {
            //            System.out.println("Result set is null.");
            //        }
            //        while (result.next()) {
            //            for (int i = 0; i<questionWords.size(); i++) {

            //                string word = result.getString(i + 1);
            //    double value = vec.similarity(word.toLowerCase(), questionWords.get(i));
            //        rowRelevance.addValue(value);

            //    }
            //}
            //        return rowRelevance.getMean();
            return default;
        }

        public static double cosineSimilarity(float[] vec1, float[] vec2)
        {

            double dotProduct = 0;
            double norm1 = 0;
            double norm2 = 0;
            for (int i = 0; i < vec1.Length; i++)
            {
                float v1 = vec1[i];
                float v2 = vec2[i];
                dotProduct += vec1[i] * vec2[i];
                norm1 += v1 * v1;
                norm2 += v2 * v2;
            }
            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }

        public double cosineSimilarity(string word1, string word2)
        {
            word1 = word1?.ToLower() ?? throw new ArgumentNullException(nameof(word1));
            word2 = word2?.ToLower() ?? throw new ArgumentNullException(nameof(word2));

            //check equality
            if (string.Equals(word1, word2, StringComparison.Ordinal))
            {
                return 1;
            }

            //check numbers
            if (int.TryParse(word1, out int num1) && int.TryParse(word2, out int num2))
            {
                if (num1 >= 1700 && num1 <= 2020 && num2 >= 1700 && num2 <= 2020)
                {
                    return Math.Abs(num1 - num2);
                }
            }

            //check WORDSTAGS
            if ((KeywordsTags.TryGetValue(word1, out string w1s) && w1s == "NNP") ||
                (KeywordsTags.TryGetValue(word2, out string w2s) && w2s == "NNP"))
            {
                //words are already checked to be not equal
                return -1;
            }

            //check vector similarity
            if (Word2Vec.TryGetValue(word1, out var vec1) && Word2Vec.TryGetValue(word2, out var vec2))
            {
                return cosineSimilarity(vec1, vec2);
            }

            //cannot be compared
            Console.WriteLine($"The word \"{word1}\" or \"{word2}\" is not in conceptnet.");
            return -1;
        }

        public double[] Evaluate(StubIndividual individual)
        {
            var sql = individual.ToSql();














            var result = new[] { 0.0, 0.0 };
            return result;
            //Evaluate sql statement against DB

            //        try {
            //            //Get db connection
            //            //Get db statement

            //        //Generate full SQL from the program
            //        //Get resultset
            //        //Look at DBMiner code do some stuff
            //        Statement st = DBAccess.getConnection().createStatement();

            //        List<string> initialColumns = getCols(stringProgramGene);

            //        stringProgramGene.eval();

            //            string sql = "SELECT " + string.join(",", initialColumns) + " FROM " + Util.tablename + " WHERE " + stringProgramGene.eval() + ";";
            //        ResultSet rs = st.executeQuery(sql);

            //        ResultSetMetaData rsmd = rs.getMetaData();
            //        int cols = rsmd.getColumnCount();
            //        string[] columns = new string[cols];

            //        //Initialise these to fixed len arrays which we will know getColumnnCount()
            //        //Loop through the columns from the metadata

            //        DescriptiveStatistics colRelevance = new DescriptiveStatistics();
            //            for (int i = 0; i<cols; i++) {
            //                columns[i] = rsmd.getColumnName(i+1).toLowerCase().replaceAll("'", "");
            //                for (int j =0; j<questionWords.size(); j++) {
            //                    colRelevance.addValue(vec.similarity(columns[i], questionWords.get(j)));
            //                }
            //}

            ////Implement row relevance
            //double fitness = colRelevance.getMean() * getRowsRelevance(rs);
            //System.out.printf("Fitness: %f\n", fitness);
            //            return fitness;
            //        } catch (SQLException e) {
            //            e.printStackTrace();
            //        } catch (ClassNotFoundException e) {
            //            e.printStackTrace();
            //        }

            throw new NotImplementedException();
        }

    }

}
