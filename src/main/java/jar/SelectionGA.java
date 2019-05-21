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
import java.util.Arrays;
import java.util.function.Supplier;

import static java.lang.Math.abs;

public class SelectionGA {

    /**
     * Runs test of tree chromosomes using the Jenetics interface Op
     */
    static final String[][] SAMPLES = new String[][] {
            {"test", "test"},
            {"test", "test"}
    };

    static double fitness(final ProgramGene<String> program) {
        //TODO Replace random fitness function with real fitness function
        return (double) RandomRegistry.getRandom().nextInt(1);
    }


    /**
     * Defines the logic required to concatenate statements. These are the 'Operations'
     *
     * Each of these are doing the set logic behind the GA. Each of these will combine with a 'final'
     * which we will use to create branches in teh tree, which is why they have two options
     */
    final static Op<String> AND = Op.of("AND", 2, v -> " (" + v[0] + " AND " + v[1] + ") ");
    final static Op<String> OR = Op.of("OR", 2, v -> " (" + v[0] + " OR " + v[1] + ") ");
    final static ISeq<Op<String>> operations = ISeq.of(AND, OR);

    //TODO: What are these .of methods?

    /**
     * Definition of all terminal operations
     *
     * This is described in terms of a tree, so that these @see io.jenetics.prog.ProgramChromosome
     * are all the leaves of the trees and can be combined relatively easily. Further up the tree we
     * have the 'Operations' which combine these.
     */

     /*
     TODO Write a java.util.function.supplier factory which takes the sql operator
     which will give column names in the first field, and then the acceptable values
     in the constraint (right hand side)
     */

    //We know there are specific rules about which is able to go on the left and the right here
    // perhaps there is a more general way to capture this in an AST?
    // Can fix with a "Fix" step for the ga
            final static String[] cols = {"Col1", "Col2", "Col3"};
            final static String[][] data = {{""}};

    final static Supplier TestStringSupplier = () -> DataGetter.makePredicate();
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


        final Engine<ProgramGene<String>, Double> engine = Engine
                .builder(SelectionGA::fitness, CODEC)
                .minimizing()
                .alterers(
                        new SingleNodeCrossover<>(),
                        new Mutator<>()
                )
                .build();
        final ProgramGene<String> prog = engine.stream()
                .limit(300)
                .collect(EvolutionResult.toBestGenotype())
                .getGene();
        final String result = prog.eval();

    }

}
