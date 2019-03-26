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
        //QuestionParser parser = new QuestionParser(question.toString());
        //ArrayList keywords = parser.keywords();
        //System.out.println(keywords);
        //NumberBatch num = new NumberBatch();
        //System.out.println("Done!");

        final GensimTest problem = new GensimTest(150, 0.01);
        final Engine<BitGene, Integer> engine = Engine.builder(problem).build();

        final ISeq<BitGene> result = problem.codec().decoder().apply(
                engine.stream()
                        .limit(10)
                        .collect(EvolutionResult.toBestGenotype())
        );

        System.out.println(result);
    }
}