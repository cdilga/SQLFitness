package jar;

import io.jenetics.BitGene;
import io.jenetics.engine.Engine;
import io.jenetics.engine.EvolutionResult;
import io.jenetics.util.ISeq;

import java.util.ArrayList;

public class SQLFitness {

    public static void main(String[] args) {
        StringBuilder question = new StringBuilder();
        for (int i = 0;i<args.length;i++)
        {
            question.append(args[i] + ' ');
        }
        //Use the apache downloader which downloads stuff from the DL4J examples to download the github release for
        //conceptnet automagically. Potentially should download when it's built with maven....
        //

        //QuestionParser parser = new QuestionParser(question.toString());
        //ArrayList keywords = parser.keywords();
        //System.out.println(keywords);
        //NumberBatch num = new NumberBatch();
        //System.out.println("Done!");

    }
}
