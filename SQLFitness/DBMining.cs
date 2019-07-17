//using edu.stanford.nlp.ling;
//using edu.stanford.nlp.pipeline;
//using edu.stanford.nlp.trees;
//using edu.stanford.nlp.util;
//using java.net;
//using java.sql;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using static edu.stanford.nlp.ling.CoreAnnotations;

//namespace SQLFitness
//{
//    class DBMining
//    {
//        public List<string> rows = new List<string>();
//        public string query;
//        public List<string> QueryWords;
//        public List<relevance> queryColumnsRel;

//        // "How many discoveries Martin has made after 1990?";
//        public static string question = "How many discoveries Martin has made after 1990?";
//        public static string groundTruthQuery;
//        public static List<string> QuestionWords;
//        public static IReadOnlyDictionary<string, float[]> Vectors;
//        public Statement stmt;
//        public static bool storeRelevanceScores = false;
//        public static List<string> SQL = new List<string>();
//        public static ConcurrentDictionary<string, string> WORDSTAGS = new ConcurrentDictionary<string, string>();
//        public static Dictionary<string, bool> testedQueries = new Dictionary<string, bool>();
//        public static TextWriter bw;
//        public static Dictionary<string, Dictionary<string, string>> ALL = new Dictionary<string, Dictionary<string, string>>();
//        public static Dictionary<string, Dictionary<string, string>> GT = new Dictionary<string, Dictionary<string, string>>();


//        //Input Question.
//        public static void main(string[] args) {
//            bw = new StreamWriter("coverages.csv") {
//                AutoFlush = true
//            };

//            if(args.Length == 2) {
//                question = args[0];
//                groundTruthQuery = args[1];
//            }
//            else {
//                Console.WriteLine("Some of the inputs are missing!");
//                return;
//            }
//            try {
//                loadNumberBatch();
//                QuestionWords = extractQuestionWords();
//            }
//            catch(SqlException ex) {
//                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
//            }

//            try {
//                MyService = new ServerSocket(1506);
//            }
//            catch(IOException ex) {
//                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
//            }
//            Console.WriteLine("Question is: " + question);
//            //Console.WriteLine("Now enter the best query for this question:");
//            //Scanner scanner = new Scanner(System.in);
//            //string groundTruthQuery = scanner.nextLine();
//            Console.WriteLine("The ground truth has a fitness of: " + getQueryFitness(groundTruthQuery));
//            storeRelevanceScores = true;
//            Console.WriteLine("The fitness for all the tables is: " + getQueryFitness("SELECT * FROM species.insectdiscoveries;"));
//            storeRelevanceScores = false;
//            saveRelevanceScores();
//            Console.WriteLine("Press any key to continue...");
//            Console.ReadKey();
//            Console.WriteLine("Coverage of ground truth vs all table is: " + getQueryCoverage(groundTruthQuery, "SELECT * FROM species.insectdiscoveries;"));
//            //=========================================================================================
//            DBMining dbb = new DBMining();
//            ResultSet result = dbb.getResultSet("SELECT * FROM species.insectdiscoveries;");
//            ResultSetMetaData rsmd = result.getMetaData();
//            while(result.next()) {
//                string ID = result.getString(1);
//                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
//                    string name = rsmd.getColumnName(i);
//                    ALL.put(ID, name, result.getString(name));
//                }
//            }
//            //=========================================================================================
//            result = dbb.getResultSet(groundTruthQuery);
//            rsmd = result.getMetaData();
//            while(result.next()) {
//                string ID = result.getString(1);
//                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
//                    string name = rsmd.getColumnName(i);
//                    GT.put(ID, name, result.getString(name));
//                }
//            }
//            //=========================================================================================
//            //=========================================================================================
//            //-----------------------------------------------------------------------------------------
//            int threads = 8;
//            DBMining[] db = new DBMining[8];
//            Thread[] t = new Thread[threads];
//            for(int i = 0; i < threads; i++) {
//                db[i] = new DBMining();
//                t[i] = new Thread(db[i]);
//                t[i].Start();
//            }
//            //-----------------------------------------------------------------------------------------
//        }

//        public static void saveRelevanceScores() {
//            Statement s = OpenConnectionMYSQL("species");
//            s.executeUpdate("truncate table insectdiscoveriesrel;");
//            for(int i = 0; i < SQL.size(); i++) {
//                //Console.WriteLine(SQL.get(i));
//                s.executeUpdate(SQL.get(i));
//            }
//            s.close();
//        }

//        public static double getQueryCoverage(string groundTruthQuery, string query) {
//            query = query.Replace("SELECT ", "SELECT `ID`,");
//            //=========================================================================================
//            var ALL = new Dictionary<string, Dictionary<string, string>>();
//            DBMining db = new DBMining();
//            ResultSet result = db.getResultSet("SELECT * FROM species.insectdiscoveries;");
//            ResultSetMetaData rsmd = result.getMetaData();
//            while(result.next()) {
//                string ID = result.getString(1);
//                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
//                    string name = rsmd.getColumnName(i);
//                    ALL.put(ID, name, result.getString(name));
//                }
//            }
//            //=========================================================================================
//            var GT = new Dictionary<string, Dictionary<string, string>>();
//            result = db.getResultSet(groundTruthQuery);
//            rsmd = result.getMetaData();
//            while(result.next()) {
//                string ID = result.getString(1);
//                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
//                    string name = rsmd.getColumnName(i);
//                    GT.put(ID, name, result.getString(name));
//                }
//            }
//            //=========================================================================================
//            var Q = new Dictionary<string, Dictionary<string, string>>();
//            result = db.getResultSet(query);
//            rsmd = result.getMetaData();
//            while(result.next()) {
//                string ID = result.getString(1);
//                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
//                    string name = rsmd.getColumnName(i);
//                    Q.put(ID, name, result.getString(name));
//                }
//            }
//            //=========================================================================================

//            double FP = 0;
//            double FN = 0;
//            double N = 0;
//            foreach(var column in ALL) {
//                foreach(var rowKey in column.Value.Keys) {
//                    string colKey = column.Key;
//                    if(GT.contains(rowKey, colKey) && Q.contains(rowKey, colKey)) {
//                    }
//                    else if(!GT.contains(rowKey, colKey) && Q.contains(rowKey, colKey)) {
//                        FP++;
//                    }
//                    else if(GT.contains(rowKey, colKey) && !Q.contains(rowKey, colKey)) {
//                        FN++;
//                    }
//                    else if(!GT.contains(rowKey, colKey) && !Q.contains(rowKey, colKey)) {
//                    }
//                    N++;
//                }
//            }
//            //=========================================================================================
//            return 1 - FP / N - (2 * FN / N);
//        }

//        public double getQueryCoverage() {
//            //=========================================================================================
//            var QUERY = new Dictionary<string, Dictionary<string, string>>();
//            ResultSet result = this.getResultSet(query);
//            ResultSetMetaData rsmd = result.getMetaData();
//            while(result.next()) {
//                string ID = result.getstring(1);
//                for(int i = 1; i <= rsmd.getColumnCount(); i++) {
//                    string name = rsmd.getColumnName(i);
//                    QUERY.put(ID, name, result.getstring(name));
//                }
//            }
//            //=========================================================================================
//            Iterator it = ALL.cellSet().iterator();
//            double FP = 0;
//            double FN = 0;
//            double N = ALL.size();
//            while(it.hasNext()) {
//                Cell<string, string, string> c = (Cell)it.next();
//                string rowKey = c.getRowKey();
//                string colKey = c.getColumnKey();
//                if(GT.contains(rowKey, colKey) && QUERY.contains(rowKey, colKey)) {
//                }
//                else if(!GT.contains(rowKey, colKey) && QUERY.contains(rowKey, colKey)) {
//                    FP++;
//                }
//                else if(GT.contains(rowKey, colKey) && !QUERY.contains(rowKey, colKey)) {
//                    FN++;
//                }
//                else if(!GT.contains(rowKey, colKey) && !QUERY.contains(rowKey, colKey)) {
//                }
//            }
//            //=========================================================================================
//            return 1 - ((double)FP / N) - (2 * (double)FN / N);
//        }

//        public static double getQueryFitness(string query) {
//            DBMining db = new DBMining(query);
//            return db.getFitness();
//        }

//        public static ServerSocket MyService;

//        public void run() {
//            try {

//                while(true) {
//                    Socket mySocket = MyService.accept();
//                    Console.WriteLine("Connected");
//                    var input = new java.io.DataInputStream(mySocket.getInputStream());
//                    var r = new java.io.BufferedReader(new java.io.InputStreamReader(input));
//                    string st = r.readLine();
//                    Console.WriteLine(st);
//                    if(String.Equals(st, "DIE", StringComparison.InvariantCultureIgnoreCase)) {
//                        break;
//                    }
//                    this.setQuery(st);
//                    string response = "0\n";
//                    double fitness = 0;

//                    query = query.Replace("SELECT ", "SELECT `ID`,");
//                    double coverage = this.getQueryCoverage();
//                    Console.WriteLine("Fitness : " + fitness + "---" + "Coverage: " + coverage);
//                    if(!testedQueries.ContainsKey(query)) {
//                        lock(this) {
//                            bw.WriteLine("\"" + query + "\",\"" + fitness + "\",\"" + coverage + "\"");
//                        }
//                    }
//                    testedQueries.Add(query, true);

//                    try {
//                        fitness = getFitness();
//                        response = fitness + "\n" + coverage + "\n";
//                    }
//                    catch(Exception e) {
//                        Console.WriteLine("Query was not valid. Using zero fitness instead!");
//                        Console.WriteLine(e.ToString());
//                        //System.in.read();
//                    }

//                    //send the response to the client and close the stream as well as the socket.
//                    DataOutputStream output = new DataOutputStream(mySocket.getOutputStream());
//                    output.writeUTF(response);
//                    output.flush();
//                    output.close();
//                    mySocket.close();
//                }
//            }
//            catch(IOException ex) {
//                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
//            }
//            catch(SQLException ex) {
//                Logger.getLogger(nameof(DBMining)).log(Level.SEVERE, null, ex);
//            }
//        }

//        private void setQuery(string quer) {
//            query = quer;
//        }

//        private DBMining() {
//            stmt = OpenConnectionMYSQL("species");
//        }

//        private DBMining(string quer) {
//            query = quer;
//            stmt = OpenConnectionMYSQL("species");
//        }

//        public double getFitness() {
//            QueryWords = extractQueryColumns(query);
//            Console.WriteLine("---------------------------------------------------");
//            Console.WriteLine("Query: " + QueryWords);
//            Console.WriteLine("Question: " + QuestionWords);
//            queryColumnsRel = getColumnsRelevance();
//            return Fitness(query, queryColumnsRel);
//        }

//        public List<List<relevance>> getRowsRelevance() {
//            var str = new List<List<relevance>>();
//            ResultSet result = getResultSet(query);
//            if(result == null) {
//                Console.WriteLine("Result set is null.");
//            }
//            while(result.next()) {
//                var temp = new List<relevance>();
//                string[] t = new string[QueryWords.Count];
//                for(int i = 1; i <= QueryWords.Count; i++) {
//                    relevance rel = new relevance();
//                    rel.word = result.getstring(i + 1);
//                    rel.value = cellRelevanceScore(rel.word);
//                    //t[i - 1] = rel.word + ":" + (rel.value + queryColumnsRel.get(i - 1).value);
//                    t[i - 1] = rel.word + ":" + rel.value;
//                    temp.add(rel);
//                }
//                str.add(temp);
//                if(storeRelevanceScores) {
//                    SQL.add(createInsertQuery(t));
//                }
//            }
//            return str;
//        }

//        public List<relevance> getColumnsRelevance() {
//            var desc = new List<DescriptiveStatistics>();
//            for(int j = 0; j < QueryWords.Count; j++) {
//                var temp = new DescriptiveStatistics();
//                for(int i = 0; i < QuestionWords.Count; i++) {
//                    double sim = cosineSimilarity(QuestionWords.get(i), QueryWords.get(j));
//                    //Console.WriteLine(Question.get(i) + "," + Query.get(j) + " : " + sim);
//                    temp.addValue(sim);
//                }
//                desc.add(temp);
//            }
//            var str = new List<relevance>();
//            string[] t = new string[QueryWords.Count];
//            for(int j = 0; j < QueryWords.Count; j++) {
//                relevance rel = new relevance();
//                rel.word = QueryWords[j];
//                rel.value = desc.get(j).getMean();
//                t[j] = rel.word + ":" + rel.value;
//                str.Add(rel);
//            }
//            if(storeRelevanceScores) {
//                SQL.add(createInsertQuery(t));
//            }
//            return str;
//        }

//        public string createInsertQuery(string[] t) {
//            string sql = "Insert into " + "species.insectdiscoveriesrel";
//            sql += "( "
//                    + "`" + QueryWords[0] + "`" + ","
//                    + "`" + QueryWords[1] + "`" + ","
//                    + "`" + QueryWords[2] + "`" + ","
//                    + "`" + QueryWords[3] + "`" + ","
//                    + "`" + QueryWords[4] + "`" + ","
//                    + "`" + QueryWords[5] + "`" + ","
//                    + "`" + QueryWords[6] + "`" + ","
//                    + "`" + QueryWords[7] + "`" + ","
//                    + "`" + QueryWords[8] + "`" + ","
//                    + "`" + QueryWords[9] + "`" + ","
//                    + "`" + QueryWords[10] + "`" + ","
//                    + "`" + QueryWords[11] + "`" + ","
//                    + "`" + QueryWords[12] + "`" + ","
//                    + "`" + QueryWords[13] + "`" + ","
//                    + "`" + QueryWords[14] + "`" + ","
//                    + "`" + QueryWords[15] + "`" + ","
//                    + "`" + QueryWords[16] + "`" + ","
//                    + "`" + QueryWords[17] + "`" + ","
//                    + "`" + QueryWords[18] + "`" + ","
//                    + "`" + QueryWords[19] + "`" + ","
//                    + "`" + QueryWords[20] + "`" + ","
//                    + "`" + QueryWords[21] + "`"
//                    + " )"
//                    + "Values ( "
//                    + " '" + t[0] + "', "
//                    + " '" + t[1] + "', "
//                    + " '" + t[2] + "', "
//                    + " '" + t[3] + "', "
//                    + " '" + t[4] + "', "
//                    + " '" + t[5] + "', "
//                    + " '" + t[6] + "', "
//                    + " '" + t[7] + "', "
//                    + " '" + t[8] + "', "
//                    + " '" + t[9] + "', "
//                    + " '" + t[10] + "', "
//                    + " '" + t[11] + "', "
//                    + " '" + t[12] + "', "
//                    + " '" + t[13] + "', "
//                    + " '" + t[14] + "', "
//                    + " '" + t[15] + "', "
//                    + " '" + t[16] + "', "
//                    + " '" + t[17] + "', "
//                    + " '" + t[18] + "', "
//                    + " '" + t[19] + "', "
//                    + " '" + t[20] + "', "
//                    + " '" + t[21] + "' "
//                    + " );";
//            return sql;
//        }

//        public class relevance
//        {

//            public string word;
//            public double value;
//        }

//        public double cellRelevanceScore(string cell) {
//            var temp = new DescriptiveStatistics();
//            foreach(var questionWord in QuestionWords) {
//                double sim = 0;
//                try {
//                    sim = cosineSimilarity(questionWord, cell);
//                }
//                catch(Exception) {
//                    Console.WriteLine("calculating sim failed.");
//                }
//                temp.addValue(sim);
//            }
//            return temp.getMean();
//        }

//        public class TableCell
//        {

//            int ID;
//            string Name;
//            string Value;
//            double Relevance;

//            public TableCell(int id, string name, string value) {
//                ID = id;
//                Name = name;
//                Value = value;
//            }

//            public void setcellRelevanceScore() {
//                var temp = new DescriptiveStatistics();
//                foreach(string questionWord in QuestionWords) {
//                    double sim = 0;
//                    try {
//                        sim = cosineSimilarity(questionWord, Value);
//                    }
//                    catch(Exception) {
//                        Console.WriteLine("calculating sim failed.");
//                    }
//                    temp.addValue(sim);
//                }
//                Relevance = temp.getMean();
//            }
//        }

//        //================================================================================================
//        public static List<string> extractQuestionWords() {
//            var QuestionColumns = parseText(question);
//            var temp = new List<string>();
//            foreach(var (column, tag) in QuestionColumns) {
//                Console.WriteLine(column + " " + tag);
//                if(tag.Contains("CD")) {
//                    int num = Int32.Parse(column);
//                    if(num >= 1700 && num <= 2020) {
//                        temp.Add(column);
//                    }
//                }
//                else if(tag.Contains("NN")) {
//                    temp.Add(column);
//                }
//            }
//            return temp;
//        }

//        public double Fitness(string query, List<relevance> colrel) {
//            //Console.WriteLine(colrel);
//            List<List<relevance>> rowsrel = getRowsRelevance();
//            //Console.WriteLine(rowsrel);
//            var stat = new DescriptiveStatistics();
//            foreach(var rowrel in rowsrel) {
//                double temp = 0;
//                for(int j = 0; j < colrel.Count; j++) {
//                    double d1 = colrel[j].value;
//                    double d2 = rowrel[j].value;
//                    temp += d1 + d2;
//                }
//                stat.addValue(temp);
//            }
//            double output = (double)stat.getSum();
//            return Double.IsNaN(output) ? 0.0 : output;
//        }

//        public List<string> extractQueryColumns(string query) {
//            ResultSet result = getResultSet(query);
//            Console.WriteLine("Result set ready.");
//            var ResultsColumns = new List<string>();
//            ResultSetMetaData rsmd = result.getMetaData();
//            for(int i = 2; i <= rsmd.getColumnCount(); i++) {
//                string name = rsmd.getColumnName(i);
//                //Console.WriteLine(name);
//                ResultsColumns.Add(name);
//            }
//            return ResultsColumns;
//        }


//        public ResultSet getResultSet(string query) {
//            lock(stmt) {
//                return stmt.executeQuery(query);
//            }
//        }





//        readonly static java.lang.Class sentencesAnnotationClass =
//            new CoreAnnotations.SentencesAnnotation().getClass();
//        readonly static java.lang.Class tokensAnnotationClass =
//            new CoreAnnotations.TokensAnnotation().getClass();
//        readonly static java.lang.Class textAnnotationClass =
//            new CoreAnnotations.TextAnnotation().getClass();
//        readonly static java.lang.Class partOfSpeechAnnotationClass =
//            new CoreAnnotations.PartOfSpeechAnnotation().getClass();
//        readonly static java.lang.Class namedEntityTagAnnotationClass =
//            new CoreAnnotations.NamedEntityTagAnnotation().getClass();
//        readonly static java.lang.Class normalizedNamedEntityTagAnnotation =
//            new CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

//        public static Dictionary<string, string> parseText(string text) {
//            var output = new Dictionary<string, string>();
//            // creates a StanfordCoreNLP object, with POS tagging, lemmatization, NER, parsing, and coreference resolution 
//            var props = new java.util.Properties();
//            props.put("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
//            var pipeline = new StanfordCoreNLP(props);

//            // create an empty Annotation just with the given text
//            var document = new Annotation(text);

//            // run all Annotators on this text
//            pipeline.annotate(document);

//            // these are all the sentences in this document
//            // a CoreMap is essentially a Map that uses class objects as keys and has values with custom types
//            var sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;

//            foreach(CoreMap sentence in sentences) {
//                // traversing the words in the current sentence
//                // a CoreLabel is a CoreMap with additional token-specific methods
//                foreach(CoreLabel token in sentence.get(tokensAnnotationClass) as java.util.AbstractList) {
//                    // this is the text of the token
//                    var word = token.get(textAnnotationClass);
//                    // this is the POS tag of the token
//                    var pos = token.get(partOfSpeechAnnotationClass);
//                    //WORDSTAGS.put(word, pos);
//                    //output.put(word, pos);
//                    // this is the NER label of the token
//                    //var ne = token.get(namedEntityTagAnnotationClass);
//                }

//                // this is the parse tree of the current sentence
//                //Tree tree = sentence.get(typrof(TreeAnnotation));

//                // this is the Stanford dependency graph of the current sentence
//                //SemanticGraph dependencies = sentence.get(typrof(CollapsedCCProcessedDependenciesAnnotation));
//            }

//            // This is the coreference link graph
//            // Each chain stores a set of mentions that link to each other,
//            // along with a method for getting the most representative mention
//            // Both sentence and token offsets start at 1!
//            //Map<Integer, CorefChain> graph = document.get(typeof(CorefChainAnnotation));
//            return output;
//        }

//        public static Statement OpenConnectionMYSQL(string Dataset) {
//            string url = "jdbc:mysql://localhost:3306/" + Dataset + "?useSSL=false";
//            string username = "root";
//            string password = "farhad";
//            Connection connection = DriverManager.getConnection(url, username, password);
//            Statement stmt = connection.createStatement();
//            return stmt;
//        }

//        //================================================================================================

//        public static double cosineSimilarity(string word1, string word2) {
//            word1 = word1?.ToLower() ?? throw new ArgumentNullException(nameof(word1));
//            word2 = word2?.ToLower() ?? throw new ArgumentNullException(nameof(word2));

//            //check equality
//            if(String.Equals(word1, word2, StringComparison.Ordinal)) {
//                return 1;
//            }

//            //check numbers
//            if(Int32.TryParse(word1, out int num1) && Int32.TryParse(word2, out int num2)) {
//                if(num1 >= 1700 && num1 <= 2020 && num2 >= 1700 && num2 <= 2020) {
//                    return Math.Abs(num1 - num2);
//                }
//            }

//            //check WORDSTAGS
//            if((WORDSTAGS.TryGetValue(word1, out string w1s) && w1s == "NNP") ||
//                (WORDSTAGS.TryGetValue(word2, out string w2s) && w2s == "NNP")) {
//                //words are already checked to be not equal
//                return -1;
//            }

//            //check vector similarity
//            if(Vectors.TryGetValue(word1, out var vec1) && Vectors.TryGetValue(word2, out var vec2)) {
//                return cosineSimilarity(vec1, vec2);
//            }

//            //cannot be compared
//            Console.WriteLine($"The word \"{word1}\" or \"{word2}\" is not in conceptnet.");
//            return -1;
//        }

//        public static double cosineSimilarity(float[] vec1, float[] vec2) {

//            double dotProduct = 0;
//            double norm1 = 0;
//            double norm2 = 0;
//            for(int i = 0; i < vec1.Length; i++) {
//                float v1 = vec1[i];
//                float v2 = vec2[i];
//                dotProduct += vec1[i] * vec2[i];
//                norm1 += v1 * v1;
//                norm2 += v2 * v2;
//            }
//            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
//        }

//        public static double euclideanSimilarity(string word1, string word2) {
//            word1 = word1?.ToLower() ?? throw new ArgumentNullException(nameof(word1));
//            word2 = word2?.ToLower() ?? throw new ArgumentNullException(nameof(word2));

//            if(Vectors.TryGetValue(word1, out var vec1) && Vectors.TryGetValue(word2, out var vec2)) {
//                return euclideanSimilarity(vec1, vec2);
//            }
//            else {
//                Console.WriteLine($"The word \"{word1}\" or \"{word2}\" is not in conceptnet.");
//                return -1;
//            }
//        }

//        public static double euclideanSimilarity(float[] vec1, float[] vec2) {
//            double diff_square_sum = 0.0;
//            for(int i = 0; i < vec1.Length; i++) {
//                float diff = vec1[i] - vec2[i];
//                diff_square_sum += diff * diff;
//            }
//            return Math.Sqrt(diff_square_sum);
//        }
//    }
//}