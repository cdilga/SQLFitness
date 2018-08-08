using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using java.net;
using java.sql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static edu.stanford.nlp.ling.CoreAnnotations;

namespace SQLFitness
{
    class DBMining
    {
        public List<string> rows = new List<string>();
        public string query;
        public List<string> QueryWords;
        public List<relevance> queryColumnsRel;

        // "How many discoveries Martin has made after 1990?";
        public static string question = "How many discoveries Martin has made after 1990?";
        public static string groundTruthQuery;
        public static List<string> QuestionWords;
        public static IReadOnlyDictionary<string, float[]> Vectors;
        public Statement stmt;
        public static bool storeRelevanceScores = false;
        public static List<string> SQL = new List<string>();
        public static ConcurrentDictionary<string, string> WORDSTAGS = new ConcurrentDictionary<string, string>();
        public static Dictionary<string, bool> testedQueries = new Dictionary<string, bool>();
        public static TextWriter bw;
        public static Dictionary<string, Dictionary<string, string>> ALL = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> GT = new Dictionary<string, Dictionary<string, string>>();


        //Input Question.
        public static void main(string[] args) {
            bw = new StreamWriter("coverages.csv") {
                AutoFlush = true
            };

            if(args.Length == 2) {
                question = args[0];
                groundTruthQuery = args[1];
            }
            else {
                Console.WriteLine("Some of the inputs are missing!");
                return;
            }
            try {
                loadNumberBatch();
                QuestionWords = extractQuestionWords();
            }
            catch(SqlException ex) {
                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
            }

            try {
                MyService = new ServerSocket(1506);
            }
            catch(IOException ex) {
                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
            }
            Console.WriteLine("Question is: " + question);
            //Console.WriteLine("Now enter the best query for this question:");
            //Scanner scanner = new Scanner(System.in);
            //string groundTruthQuery = scanner.nextLine();
            Console.WriteLine("The ground truth has a fitness of: " + getQueryFitness(groundTruthQuery));
            storeRelevanceScores = true;
            Console.WriteLine("The fitness for all the tables is: " + getQueryFitness("SELECT * FROM species.insectdiscoveries;"));
            storeRelevanceScores = false;
            saveRelevanceScores();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine("Coverage of ground truth vs all table is: " + getQueryCoverage(groundTruthQuery, "SELECT * FROM species.insectdiscoveries;"));
            //=========================================================================================
            DBMining dbb = new DBMining();
            ResultSet result = dbb.getResultSet("SELECT * FROM species.insectdiscoveries;");
            ResultSetMetaData rsmd = result.getMetaData();
            while(result.next()) {
                string ID = result.getstring(1);
                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
                    string name = rsmd.getColumnName(i);
                    ALL.put(ID, name, result.getstring(name));
                }
            }
            //=========================================================================================
            result = dbb.getResultSet(groundTruthQuery);
            rsmd = result.getMetaData();
            while(result.next()) {
                string ID = result.getstring(1);
                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
                    string name = rsmd.getColumnName(i);
                    GT.put(ID, name, result.getstring(name));
                }
            }
            //=========================================================================================
            //=========================================================================================
            //-----------------------------------------------------------------------------------------
            int threads = 8;
            DBMining[] db = new DBMining[8];
            Thread[] t = new Thread[threads];
            for(int i = 0; i < threads; i++) {
                db[i] = new DBMining();
                t[i] = new Thread(db[i]);
                t[i].start();
            }
            //-----------------------------------------------------------------------------------------
        }

        public static void saveRelevanceScores() {
            Statement s = OpenConnectionMYSQL("species");
            s.executeUpdate("truncate table insectdiscoveriesrel;");
            for(int i = 0; i < SQL.size(); i++) {
                //Console.WriteLine(SQL.get(i));
                s.executeUpdate(SQL.get(i));
            }
            s.close();
        }

        public static double getQueryCoverage(string groundTruthQuery, string query) {
            query = query.replaceAll("SELECT ", "SELECT `ID`,");
            //=========================================================================================
            var ALL = new Dictionary<string, Dictionary<string, string>>();
            DBMining db = new DBMining();
            ResultSet result = db.getResultSet("SELECT * FROM species.insectdiscoveries;");
            ResultSetMetaData rsmd = result.getMetaData();
            while(result.next()) {
                string ID = result.getstring(1);
                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
                    string name = rsmd.getColumnName(i);
                    ALL.put(ID, name, result.getstring(name));
                }
            }
            //=========================================================================================
            var GT = new Dictionary<string, Dictionary<string, string>>();
            result = db.getResultSet(groundTruthQuery);
            rsmd = result.getMetaData();
            while(result.next()) {
                string ID = result.getstring(1);
                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
                    string name = rsmd.getColumnName(i);
                    GT.put(ID, name, result.getstring(name));
                }
            }
            //=========================================================================================
            var Q = new Dictionary<string, Dictionary<string, string>>();
            result = db.getResultSet(query);
            rsmd = result.getMetaData();
            while(result.next()) {
                string ID = result.getstring(1);
                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
                    string name = rsmd.getColumnName(i);
                    Q.put(ID, name, result.getstring(name));
                }
            }
            //=========================================================================================

            double FP = 0;
            double FN = 0;
            double N = 0;
            foreach(var column in ALL) {
                foreach(var rowKey in column.Value.Keys) {
                    string colKey = column.Key;
                    if(GT.contains(rowKey, colKey) && Q.contains(rowKey, colKey)) {
                    }
                    else if(!GT.contains(rowKey, colKey) && Q.contains(rowKey, colKey)) {
                        FP++;
                    }
                    else if(GT.contains(rowKey, colKey) && !Q.contains(rowKey, colKey)) {
                        FN++;
                    }
                    else if(!GT.contains(rowKey, colKey) && !Q.contains(rowKey, colKey)) {
                    }
                    N++;
                }
            }
            //=========================================================================================
            return 1 - FP / N - (2 * FN / N);
        }

        public double getQueryCoverage() {
            //=========================================================================================
            var QUERY = new Dictionary<string, Dictionary<string, string>>();
            ResultSet result = this.getResultSet(query);
            ResultSetMetaData rsmd = result.getMetaData();
            while(result.next()) {
                string ID = result.getstring(1);
                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
                    string name = rsmd.getColumnName(i);
                    QUERY.put(ID, name, result.getstring(name));
                }
            }
            //=========================================================================================
            Iterator it = ALL.cellSet().iterator();
            double FP = 0;
            double FN = 0;
            double N = ALL.size();
            while(it.hasNext()) {
                Cell<string, string, string> c = (Cell)it.next();
                string rowKey = c.getRowKey();
                string colKey = c.getColumnKey();
                if(GT.contains(rowKey, colKey) && QUERY.contains(rowKey, colKey)) {
                }
                else if(!GT.contains(rowKey, colKey) && QUERY.contains(rowKey, colKey)) {
                    FP++;
                }
                else if(GT.contains(rowKey, colKey) && !QUERY.contains(rowKey, colKey)) {
                    FN++;
                }
                else if(!GT.contains(rowKey, colKey) && !QUERY.contains(rowKey, colKey)) {
                }
            }
            //=========================================================================================
            return 1 - ((double)FP / N) - (2 * (double)FN / N);
        }

        public static double getQueryFitness(string query) {
            DBMining db = new DBMining(query);
            return db.getFitness();
        }

        public static ServerSocket MyService;

        public void run() {
            try {

                while(true) {
                    Socket mySocket = MyService.accept();
                    Console.WriteLine("Connected");
                    DataInputStream input = new DataInputStream(mySocket.getInputStream());
                    BufferedReader r = new BufferedReader(new InputStreamReader(input));
                    string st = r.readLine();
                    Console.WriteLine(st);
                    if(st.equals("DIE")) {
                        break;
                    }
                    this.setQuery(st);
                    string response = "0\n";
                    double fitness = 0;

                    query = query.replaceAll("SELECT ", "SELECT `ID`,");
                    double coverage = this.getQueryCoverage();
                    Console.WriteLine("Fitness : " + fitness + "---" + "Coverage: " + coverage);
                    if(!testedQueries.containsKey(this.query)) {
                        synchronized(this) {
                            bw.write("\"" + this.query + "\",\"" + fitness + "\",\"" + coverage + "\"\n");
                            bw.flush();
                        }
                    }
                    testedQueries.put(this.query, true);

                    try {
                        fitness = this.getFitness();
                        response = double.tostring(fitness) + "\n" + coverage + "\n";
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.getStackTrace().tostring());
                        Console.WriteLine("Query was not valid. Using zero fitness instead!");
                        //System.in.read();
                    }

                    //send the response to the client and close the stream as well as the socket.
                    DataOutputStream output = new DataOutputStream(mySocket.getOutputStream());
                    output.writeUTF(response);
                    output.flush();
                    output.close();
                    mySocket.close();
                }
            }
            catch(IOException ex) {
                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
            }
            catch(SQLException ex) {
                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
            }
        }

        private void setQuery(string quer) {
            query = quer;
        }

        private DBMining() {
            stmt = OpenConnectionMYSQL("species");
        }

        private DBMining(string quer) {
            query = quer;
            stmt = OpenConnectionMYSQL("species");
        }

        public double getFitness() {
            QueryWords = extractQueryColumns(query);
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Query: " + QueryWords);
            Console.WriteLine("Question: " + QuestionWords);
            queryColumnsRel = getColumnsRelevance();
            return Fitness(query, queryColumnsRel);
        }

        public List<List<relevance>> getRowsRelevance() {
            var str = new List<List<relevance>>();
            ResultSet result = getResultSet(query);
            if(result == null) {
                Console.WriteLine("Result set is null.");
            }
            while(result.next()) {
                List<relevance> temp = new List<relevance>();
                string[] t = new string[QueryWords.size()];
                for(int i = 1; i <= QueryWords.size(); i++) {
                    relevance rel = new relevance();
                    rel.word = result.getstring(i + 1);
                    rel.value = cellRelevanceScore(rel.word);
                    //t[i - 1] = rel.word + ":" + (rel.value + queryColumnsRel.get(i - 1).value);
                    t[i - 1] = rel.word + ":" + rel.value;
                    temp.add(rel);
                }
                str.add(temp);
                if(storeRelevanceScores) {
                    SQL.add(createInsertQuery(t));
                }
            }
            return str;
        }

        public List<relevance> getColumnsRelevance() {
            ArrayList<DescriptiveStatistics> desc = new ArrayList<DescriptiveStatistics>();
            for(int j = 0; j < QueryWords.size(); j++) {
                DescriptiveStatistics temp = new DescriptiveStatistics();
                for(int i = 0; i < QuestionWords.size(); i++) {
                    double sim = cosineSimilarity(QuestionWords.get(i), QueryWords.get(j));
                    //Console.WriteLine(Question.get(i) + "," + Query.get(j) + " : " + sim);
                    temp.addValue(sim);
                }
                desc.add(temp);
            }
            ArrayList<relevance> str = new ArrayList<>();
            string[] t = new string[QueryWords.size()];
            for(int j = 0; j < QueryWords.size(); j++) {
                relevance rel = new relevance();
                rel.word = QueryWords.get(j);
                rel.value = desc.get(j).getMean();
                t[j] = rel.word + ":" + rel.value;
                str.add(rel);
            }
            if(storeRelevanceScores) {
                SQL.add(createInsertQuery(t));
            }
            return str;
        }

        public string createInsertQuery(string[] t) {
            string sql = "Insert into " + "species.insectdiscoveriesrel";
            sql += "( "
                    + "`" + QueryWords.get(0) + "`" + ","
                    + "`" + QueryWords.get(1) + "`" + ","
                    + "`" + QueryWords.get(2) + "`" + ","
                    + "`" + QueryWords.get(3) + "`" + ","
                    + "`" + QueryWords.get(4) + "`" + ","
                    + "`" + QueryWords.get(5) + "`" + ","
                    + "`" + QueryWords.get(6) + "`" + ","
                    + "`" + QueryWords.get(7) + "`" + ","
                    + "`" + QueryWords.get(8) + "`" + ","
                    + "`" + QueryWords.get(9) + "`" + ","
                    + "`" + QueryWords.get(10) + "`" + ","
                    + "`" + QueryWords.get(11) + "`" + ","
                    + "`" + QueryWords.get(12) + "`" + ","
                    + "`" + QueryWords.get(13) + "`" + ","
                    + "`" + QueryWords.get(14) + "`" + ","
                    + "`" + QueryWords.get(15) + "`" + ","
                    + "`" + QueryWords.get(16) + "`" + ","
                    + "`" + QueryWords.get(17) + "`" + ","
                    + "`" + QueryWords.get(18) + "`" + ","
                    + "`" + QueryWords.get(19) + "`" + ","
                    + "`" + QueryWords.get(20) + "`" + ","
                    + "`" + QueryWords.get(21) + "`"
                    + " )"
                    + "Values ( "
                    + " '" + t[0] + "', "
                    + " '" + t[1] + "', "
                    + " '" + t[2] + "', "
                    + " '" + t[3] + "', "
                    + " '" + t[4] + "', "
                    + " '" + t[5] + "', "
                    + " '" + t[6] + "', "
                    + " '" + t[7] + "', "
                    + " '" + t[8] + "', "
                    + " '" + t[9] + "', "
                    + " '" + t[10] + "', "
                    + " '" + t[11] + "', "
                    + " '" + t[12] + "', "
                    + " '" + t[13] + "', "
                    + " '" + t[14] + "', "
                    + " '" + t[15] + "', "
                    + " '" + t[16] + "', "
                    + " '" + t[17] + "', "
                    + " '" + t[18] + "', "
                    + " '" + t[19] + "', "
                    + " '" + t[20] + "', "
                    + " '" + t[21] + "' "
                    + " );";
            return sql;
        }

        public class relevance
        {

            string word;
            double value;
        }

        public double cellRelevanceScore(string cell) {
            DescriptiveStatistics temp = new DescriptiveStatistics();
            for(int i = 0; i < QuestionWords.size(); i++) {
                double sim = 0;
                try {
                    sim = cosineSimilarity(QuestionWords.get(i), cell);
                }
                catch(Exception Ex) {
                    Console.WriteLine("calculating sim failed.");
                }
                temp.addValue(sim);
            }
            return temp.getMean();
        }

        public class TableCell
        {

            int ID;
            string Name;
            string Value;
            double Relevance;

            public TableCell(int id, string name, string value) {
                ID = id;
                Name = name;
                Value = value;
            }

            public void setcellRelevanceScore() {
                DescriptiveStatistics temp = new DescriptiveStatistics();
                for(int i = 0; i < QuestionWords.size(); i++) {
                    double sim = 0;
                    try {
                        sim = cosineSimilarity(QuestionWords.get(i), this.Value);
                    }
                    catch(Exception Ex) {
                        Console.WriteLine("calculating sim failed.");
                    }
                    temp.addValue(sim);
                }
                Relevance = temp.getMean();
            }
        }

        //================================================================================================
        public static List<string> extractQuestionWords() {
            Dictionary<string, string> QuestionColumns;
            var QuestionColumnsList = new List<string>();

            QuestionColumns = parseText(question);
            Iterator it = QuestionColumns.keySet().iterator();
            while(it.hasNext()) {
                QuestionColumnsList.add((string)it.next());
            }
            ArrayList<string> temp = new ArrayList<>();
            for(int i = 0; i < QuestionColumnsList.size(); i++) {
                string tag = QuestionColumns.get(QuestionColumnsList.get(i));
                Console.WriteLine(QuestionColumnsList.get(i) + " " + tag);
                if(tag.contains("CD")) {
                    int num = Integer.parseInt(QuestionColumnsList.get(i));
                    if(num >= 1700 && num <= 2020) {
                        temp.add(Integer.tostring(num));
                    }
                }
                else if(tag.contains("NN")) {
                    temp.add(QuestionColumnsList.get(i));
                }
            }
            return temp;
        }

        public double Fitness(string query, List<relevance> colrel) {
            //Console.WriteLine(colrel);
            List<List<relevance>> rowsrel = getRowsRelevance();
            //Console.WriteLine(rowsrel);
            DescriptiveStatistics stat = new DescriptiveStatistics();
            for(int i = 0; i < rowsrel.size(); i++) {
                double temp = 0;
                ArrayList<relevance> rowrel = rowsrel.get(i);
                for(int j = 0; j < colrel.size(); j++) {
                    double d1 = colrel.get(j).value;
                    double d2 = rowrel.get(j).value;
                    temp += d1 + d2;
                }
                stat.addValue(temp);
            }
            double out = (double)stat.getSum();
            return out.isNaN() ? 0.0 : out;
        }

        public ArrayList<string> extractQueryColumns(string query) {
            ResultSet result = getResultSet(query);
            Console.WriteLine("Result set ready.");
            ArrayList<string> ResultsColumns = new ArrayList<>();
            ResultSetMetaData rsmd = result.getMetaData();
            for(int i = 2; i <= rsmd.getColumnCount(); i++) {
                string name = rsmd.getColumnName(i);
                //Console.WriteLine(name);
                ResultsColumns.add(name);
            }
            return ResultsColumns;
        }

        public synchronized ResultSet getResultSet(string query) {
            return stmt.executeQuery(query);
        }





        readonly static java.lang.Class sentencesAnnotationClass =
            new CoreAnnotations.SentencesAnnotation().getClass();
        readonly static java.lang.Class tokensAnnotationClass =
            new CoreAnnotations.TokensAnnotation().getClass();
        readonly static java.lang.Class textAnnotationClass =
            new CoreAnnotations.TextAnnotation().getClass();
        readonly static java.lang.Class partOfSpeechAnnotationClass =
            new CoreAnnotations.PartOfSpeechAnnotation().getClass();
        readonly static java.lang.Class namedEntityTagAnnotationClass =
            new CoreAnnotations.NamedEntityTagAnnotation().getClass();
        readonly static java.lang.Class normalizedNamedEntityTagAnnotation =
            new CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

        public static Dictionary<string, string> parseText(string text) {
            var output = new Dictionary<string, string>();
            // creates a StanfordCoreNLP object, with POS tagging, lemmatization, NER, parsing, and coreference resolution 
            var props = new java.util.Properties();
            props.put("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            var pipeline = new StanfordCoreNLP(props);

            // create an empty Annotation just with the given text
            var document = new Annotation(text);

            // run all Annotators on this text
            pipeline.annotate(document);

            // these are all the sentences in this document
            // a CoreMap is essentially a Map that uses class objects as keys and has values with custom types
            var sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;

            foreach(CoreMap sentence in sentences) {
                // traversing the words in the current sentence
                // a CoreLabel is a CoreMap with additional token-specific methods
                foreach(CoreLabel token in sentence.get(tokensAnnotationClass) as java.util.AbstractList) {
                    // this is the text of the token
                    var word = token.get(textAnnotationClass);
                    // this is the POS tag of the token
                    var pos = token.get(partOfSpeechAnnotationClass);
                    //WORDSTAGS.put(word, pos);
                    //output.put(word, pos);
                    // this is the NER label of the token
                    //var ne = token.get(namedEntityTagAnnotationClass);
                }

                // this is the parse tree of the current sentence
                //Tree tree = sentence.get(typrof(TreeAnnotation));

                // this is the Stanford dependency graph of the current sentence
                //SemanticGraph dependencies = sentence.get(typrof(CollapsedCCProcessedDependenciesAnnotation));
            }

            // This is the coreference link graph
            // Each chain stores a set of mentions that link to each other,
            // along with a method for getting the most representative mention
            // Both sentence and token offsets start at 1!
            //Map<Integer, CorefChain> graph = document.get(typeof(CorefChainAnnotation));
            return output;
        }

        public static Statement OpenConnectionMYSQL(string Dataset) {
            string url = "jdbc:mysql://localhost:3306/" + Dataset + "?useSSL=false";
            string username = "root";
            string password = "farhad";
            Connection connection = DriverManager.getConnection(url, username, password);
            Statement stmt = connection.createStatement();
            return stmt;
        }

        //================================================================================================

        public double cosineSimilarity(string word1, string word2) {
            word1 = word1?.ToLower() ?? throw new ArgumentNullException(nameof(word1));
            word2 = word2?.ToLower() ?? throw new ArgumentNullException(nameof(word2));

            //check equality
            if(String.Equals(word1, word2, StringComparison.Ordinal)) {
                return 1;
            }

            //check numbers
            if(Int32.TryParse(word1, out int num1) && Int32.TryParse(word2, out int num2)) {
                if(num1 >= 1700 && num1 <= 2020 && num2 >= 1700 && num2 <= 2020) {
                    return Math.Abs(num1 - num2);
                }
            }

            //check WORDSTAGS
            if((WORDSTAGS.TryGetValue(word1, out string w1s) && w1s == "NNP") ||
                (WORDSTAGS.TryGetValue(word2, out string w2s) && w2s == "NNP")) {
                //words are already checked to be not equal
                return -1;
            }

            //check vector similarity
            if(Vectors.TryGetValue(word1, out var vec1) && Vectors.TryGetValue(word2, out var vec2)) {
                return cosineSimilarity(vec1, vec2);
            }

            //cannot be compared
            Console.WriteLine($"The word \"{word1}\" or \"{word2}\" is not in conceptnet.");
            return -1;
        }

        public double cosineSimilarity(float[] vec1, float[] vec2) {

            double dotProduct = 0;
            double norm1 = 0;
            double norm2 = 0;
            for(int i = 0; i < vec1.Length; i++) {
                float v1 = vec1[i];
                float v2 = vec2[i];
                dotProduct += vec1[i] * vec2[i];
                norm1 += v1 * v1;
                norm2 += v2 * v2;
            }
            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }

        public double euclideanSimilarity(string word1, string word2) {
            word1 = word1?.ToLower() ?? throw new ArgumentNullException(nameof(word1));
            word2 = word2?.ToLower() ?? throw new ArgumentNullException(nameof(word2));

            if(Vectors.TryGetValue(word1, out var vec1) && Vectors.TryGetValue(word2, out var vec2)) {
                return euclideanSimilarity(vec1, vec2);
            }
            else {
                Console.WriteLine($"The word \"{word1}\" or \"{word2}\" is not in conceptnet.");
                return -1;
            }
        }

        public double euclideanSimilarity(float[] vec1, float[] vec2) {
            double diff_square_sum = 0.0;
            for(int i = 0; i < vec1.Length; i++) {
                float diff = vec1[i] - vec2[i];
                diff_square_sum += diff * diff;
            }
            return Math.Sqrt(diff_square_sum);
        }
    }
}