package jar;

import io.jenetics.Genotype;
import io.jenetics.Mutator;
import io.jenetics.engine.Codec;
import io.jenetics.engine.Engine;
import io.jenetics.engine.EvolutionResult;
import io.jenetics.ext.SingleNodeCrossover;
import io.jenetics.prog.ProgramChromosome;
import io.jenetics.prog.ProgramGene;
import io.jenetics.prog.op.Const;
import io.jenetics.prog.op.EphemeralConst;
import io.jenetics.prog.op.Op;
import io.jenetics.util.ISeq;
import io.jenetics.util.RandomRegistry;
import scala.sys.process.ProcessBuilderImpl;

import jar.DataGetter;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.function.Supplier;

import static java.lang.Math.abs;
import jar.FitnessFunction;

public class SelectionGA {

    /**
     * Defines the logic required to concatenate statements. These are the 'Operations'
     *
     * Each of these are doing the set logic behind the GA. Each of these will combine with a 'final'
     * which we will use to create branches in teh tree, which is why they have two options
     */
    final static Op<String> AND = Op.of("AND", 2, v -> " (" + v[0] + " AND " + v[1] + ") ");
    final static Op<String> OR = Op.of("OR", 2, v -> " (" + v[0] + " OR " + v[1] + ") ");
    final static ISeq<Op<String>> operations = ISeq.of(AND, OR);

    /**
     * Definition of all terminal operations
     *
     * This is described in terms of a tree, so that these @see io.jenetics.prog.ProgramChromosome
     * are all the leaves of the trees and can be combined relatively easily. Further up the tree we
     * have the 'Operations' which combine these.
     */

    final static ISeq<EphemeralConst<String>> terminals = ISeq.of(EphemeralConst.of(DataGetter.predicateSupplier));

    final int depth = 5;

    final static Codec<ProgramGene<String>, ProgramGene<String>> CODEC =
            Codec.of(
                    Genotype.of(ProgramChromosome.of(
                            // Program tree depth.
                            5,
                            // Chromosome validator.
                            ch -> ch.getRoot().size() <= 50,
                            operations,
                            terminals
                    )),
                    Genotype::getGene
            );

    public static void main(String[] args) {
        //Parse the question possibly passed as an argument into it's constituent form
        StringBuilder question = new StringBuilder();
        for (int i = 0;i<args.length;i++)
        {
            question.append(args[i] + ' ');
        }
        //TODO: Use the apache downloader which downloads stuff from the DL4J examples to download the github release for
        //conceptnet automagically. Potentially should download when it's built with maven....


        QuestionParser parser = new QuestionParser(question.toString());
        ArrayList keywords = parser.keywords();
        System.out.println("Keywords extracted: " + keywords);

        System.out.println("Loaded numberbatch");
        //Load in numberbatch for the fitness function

        //Run the GA
        FitnessFunction fitter = new FitnessFunction(keywords);
        final Engine<ProgramGene<String>, Double> engine = Engine
                .builder(fitter.fitness, CODEC)
                .minimizing()
                .alterers(
                        new SingleNodeCrossover<>(),
                        new Mutator<>()
                )
                .build();
        final ProgramGene<String> prog = engine.stream()
                .limit(10)
                .collect(EvolutionResult.toBestGenotype())
                .getGene();
        final String result = prog.eval();
        System.out.println(result);
    }
}
