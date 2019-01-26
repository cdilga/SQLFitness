package jar;

import edu.stanford.nlp.pipeline.*;
import java.util.*;
import edu.stanford.nlp.ling.*;
import edu.stanford.nlp.util.CoreMap;


public class QuestionParser {
    //The question parser will both be responsible for all of the NLP, and for caching the result
    //A question parser will be an object that is used only once

    ArrayList<String> parse;


    public QuestionParser (String question) {
        // creates a StanfordCoreNLP object, with POS tagging, lemmatization, NER, parsing, and coreference resolution
        Properties props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
        StanfordCoreNLP pipeline = new StanfordCoreNLP(props);

        // read some text in the text variable
        String text = question;

        // create an empty Annotation just with the given text
        Annotation document = new Annotation(text);

        // run all Annotators on this text
        pipeline.annotate(document);

        // these are all the sentences in this document
        // a CoreMap is essentially a Map that uses class objects as keys and has values with custom types
        List<CoreMap> sentences = document.get(CoreAnnotations.SentencesAnnotation.class);

        for(CoreMap sentence: sentences) {
            // traversing the words in the current sentence
            // a CoreLabel is a CoreMap with additional token-specific methods
            for (CoreLabel token: sentence.get(CoreAnnotations.TokensAnnotation.class)) {
                // this is the text of the token
                String word = token.get(CoreAnnotations.TextAnnotation.class);
                // this is the POS tag of the token
                String pos = token.get(CoreAnnotations.PartOfSpeechAnnotation.class);
                // this is the NER label of the token
                String ne = token.get(CoreAnnotations.NamedEntityTagAnnotation.class);

                this.parse.add(pos);
                System.out.println(String.format("Print  word: [%s] pos: [%s] ne: [%s]", word, pos, ne));
            }
        }

    }

    public ArrayList keywords() {
        return parse;
    }
}
