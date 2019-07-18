using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            props.setProperty("ner.useSUTime", "0");
            var curDir = Environment.CurrentDirectory;
            var jarRoot = @"..\..\..\stanford-corenlp-full-2018-10-05\models";
            Directory.SetCurrentDirectory(jarRoot);
            StanfordCoreNLP pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // read some text in the text variable
            string text = question;

            // create an empty Annotation just with the given text
            Annotation document = new Annotation(text);

            // run all Annotators on this text
            pipeline.annotate(document);

            // these are all the sentences in this document
            var sentences = document.get(sentencesAnnotationClass) as AbstractList;

            var parse = new List<string>();

            foreach (CoreMap sentence in sentences)
            {
                // traversing the words in the current sentence
                // a CoreLabel has additional token-specific methods
                foreach (CoreLabel token in sentence.get(tokensAnnotationClass) as AbstractList)
                {
                    // this is the text of the token
                    string word = token.get(textAnnotationClass) as string;
                    //this is the POS tag of the token
                    string pos = token.get(partOfSpeechAnnotationClass) as string;
                    //extract the nouns

                    if (pos == "NN" || pos == "NNP" || pos == "NNS" || pos == "NNPS")
                    {
                        parse.Add(word);
                    }
                }
            }
            return parse;
        }

        private static List<string> parseQuestion(string question)
        {
            string pathString = @"..\..\..\cache\" + question.GetHashCode() + ".q";
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