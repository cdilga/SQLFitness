package jar;

import edu.stanford.nlp.coref.data.CorefChain;
import edu.stanford.nlp.pipeline.*;
import java.util.*;
import edu.stanford.nlp.ling.*;
import edu.stanford.nlp.ie.util.*;
import edu.stanford.nlp.pipeline.*;
import edu.stanford.nlp.semgraph.*;
import edu.stanford.nlp.trees.*;
import java.util.*;



public class App {

    public static void main(String[] args) {

        // creates a StanfordCoreNLP object, with POS tagging, lemmatization, NER, parsing, and coreference resolution
        Properties props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, lemma");
        StanfordCoreNLP pipeline = new StanfordCoreNLP(props);

        // read some text in the text variable
        String text = "John saved the cat today";

        // create an empty Annotation just with the given text
        Annotation document = new Annotation(text);

        // run all Annotators on this text
        List<CoreMap> sentences = document.get(SentencesAnnotation.class);
        
        pipeline.annotate(document);
        System.out.println(document.sentences().get(0).posTags());
    }

}