package jar;

import io.jenetics.prog.ProgramGene;
import org.deeplearning4j.models.embeddings.loader.WordVectorSerializer;
import org.deeplearning4j.models.word2vec.Word2Vec;

public class FitnessFunction {
    Word2Vec vec;
    public void FitnessFunction() {
        vec = WordVectorSerializer.readWord2VecModel(Util.Word2VecPath);
    }
    public static double fitness(ProgramGene<String> programGene) {
        return 0.1;
    }
}
