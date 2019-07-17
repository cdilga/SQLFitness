using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using java.net;
using java.sql;
using java.util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static edu.stanford.nlp.ling.CoreAnnotations;

namespace SQLFitness
{
    class QuestionParser
    {
        public static List<string> ParseWithCache(string question) => parseQuestion(question);

        readonly static java.lang.Class sentencesAnnotationClass =
            new SentencesAnnotation().getClass();
        readonly static java.lang.Class tokensAnnotationClass =
            new TokensAnnotation().getClass();
        readonly static java.lang.Class textAnnotationClass =
            new TextAnnotation().getClass();
        readonly static java.lang.Class partOfSpeechAnnotationClass =
            new PartOfSpeechAnnotation().getClass();
        readonly static java.lang.Class namedEntityTagAnnotationClass =
            new NamedEntityTagAnnotation().getClass();
        readonly static java.lang.Class normalizedNamedEntityTagAnnotation =
            new NormalizedNamedEntityTagAnnotation().getClass();


        public static List<string> NLPParse(string question)
        {
            // creates a StanfordCoreNLP object, with POS tagging, lemmatization, NER, parsing, and coreference resolution
            Properties props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
            var curDir = Environment.CurrentDirectory;
            var jarRoot = @"..\..\..\stanford-corenlp-full-2016-10-31";
            Directory.SetCurrentDirectory(jarRoot);
            StanfordCoreNLP pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // read some text in the text variable
            String text = question;

            // create an empty Annotation just with the given text
            Annotation document = new Annotation(text);

            // run all Annotators on this text
            pipeline.annotate(document);

            // these are all the sentences in this document
            // a CoreMap is essentially a Map that uses class objects as keys and has values with custom types
            var sentences = document.get(sentencesAnnotationClass) as AbstractList;

            var parse = new List<string>();

            foreach (CoreMap sentence in sentences) {
                // traversing the words in the current sentence
                // a CoreLabel is a CoreMap with additional token-specific methods
                foreach (CoreLabel token in sentence.get(tokensAnnotationClass) as AbstractList) {
                    // this is the text of the token
                    String word = token.get(textAnnotationClass) as String;
                    // this is the POS tag of the token
                    String pos = token.get(partOfSpeechAnnotationClass) as String;
                    //extract the nouns

                    if (pos == "NN" || pos == "NNP" || pos == "NNS" || pos == "NNPS") {
                        parse.Add(word);
                    }
                }
            }
            return parse;
        }

        private static List<string> parseQuestion(string question)
        {
            String pathString = "C:\\Users\\Chris\\Documents\\dev\\SQLFitness\\cache\\" + question.GetHashCode() + ".q";
            if (File.Exists(pathString))
            {
                return File.ReadAllLines(pathString).ToList();
            }
            else
            {
                var parsedQuestion = NLPParse(question);
                //Write to cache
                File.WriteAllLines(pathString, parsedQuestion);
                return parsedQuestion;
            }
        }
    }
}