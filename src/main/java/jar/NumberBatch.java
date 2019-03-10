package jar;

import org.deeplearning4j.models.embeddings.loader.WordVectorSerializer;
import org.deeplearning4j.models.word2vec.Word2Vec;
import org.datavec.api.util.ClassPathResource;
import org.deeplearning4j.text.sentenceiterator.BasicLineIterator;
import org.deeplearning4j.text.sentenceiterator.SentenceIterator;
import org.deeplearning4j.text.tokenization.tokenizer.preprocessor.CommonPreprocessor;
import org.deeplearning4j.text.tokenization.tokenizerfactory.DefaultTokenizerFactory;
import org.deeplearning4j.text.tokenization.tokenizerfactory.TokenizerFactory;



import java.io.File;
import java.io.IOException;
import java.util.zip.GZIPInputStream;

public class NumberBatch {
    // Class will be responsible for efficiently loading numberbatch and exposing necessary
    // Functions
    public NumberBatch() {
        //File gModel = new File("");

        //Word2Vec vec = WordVectorSerializer.readWord2VecModel("data/numberbatch-en.txt.gz", false);

        Word2Vec vec = WordVectorSerializer.readWord2VecModel("data/numberbatch-en.txt");

        System.out.println("Completed loading of Numberbatch");
    }
}
