package jar;

import edu.stanford.nlp.pipeline.*;

import java.io.*;
import java.nio.file.Files;
import java.nio.file.Path;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.*;
import edu.stanford.nlp.ling.*;
import edu.stanford.nlp.util.CoreMap;

import javax.xml.bind.DatatypeConverter;


public class QuestionParser {
    //The question parser will both be responsible for all of the NLP, and for caching the result
    //A question parser will be an object that is used only once

    ArrayList<String> parse = new ArrayList<String>();

    private Boolean WriteCache(ArrayList parsed) throws Exception {
        throw new Exception("Not Implemented");
    }

    private Boolean ReadCache(ArrayList parsed) throws Exception {
        throw new Exception("Not Implemented");
    }

    private void NLPParse(String question) {
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
                //extract the nouns

                if (pos.equals("NN") || pos.equals("NNP") || pos.equals("NNS") || pos.equals("NNPS")) {
                    parse.add(word);
                }
            }
        }

    }

    private String GenerateHash(String str) {
        MessageDigest md = null;
        try {
            md = MessageDigest.getInstance("MD5");
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        }
        md.update(str.getBytes());
        byte[] digest = md.digest();
        String myHash = DatatypeConverter
                .printHexBinary(digest).toUpperCase();
        return myHash;
    }

    public QuestionParser (String question) {
        //Test if a file exists
        String pathString = "C:\\Users\\cdilg\\Documents\\dev\\SQLFitness\\cache\\" + GenerateHash(question) + ".q";
        //Path path = (pathString);
        File cacheQuestion = new File(pathString);
        if (cacheQuestion.exists()){
            try {
                Scanner cacheReader = new Scanner(cacheQuestion);
                while (cacheReader.hasNextLine()) {
                    parse.add(cacheReader.nextLine());
                }
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }

        } else {
            NLPParse(question);
            //Write to cache

            try {
                FileWriter cacheWriter = new FileWriter(cacheQuestion);
                for(int i =0; i < parse.size(); i++) {
                    cacheWriter.write(parse.get(i) + "\n");
                }
                cacheWriter.close();

            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        //Read from cache
        //Else do the parse then



    }

    public ArrayList keywords() {
        return parse;
    }
}
