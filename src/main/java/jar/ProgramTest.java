package jar;

import io.jenetics.Genotype;
import io.jenetics.Mutator;
import io.jenetics.engine.Codec;
import io.jenetics.engine.Engine;
import io.jenetics.engine.EvolutionResult;
import io.jenetics.ext.SingleNodeCrossover;
import io.jenetics.ext.util.Tree;
import io.jenetics.prog.ProgramChromosome;
import io.jenetics.prog.ProgramGene;
import io.jenetics.prog.op.EphemeralConst;
import io.jenetics.prog.op.MathOp;
import io.jenetics.prog.op.Op;
import io.jenetics.prog.op.Var;
import io.jenetics.util.ISeq;
import io.jenetics.util.RandomRegistry;

import java.util.Arrays;

import static java.lang.Math.abs;

public class ProgramTest {
    static final ISeq<Op<Double>> OPERATIONS = ISeq.of(
            MathOp.ADD,
            MathOp.SUB,
            MathOp.MUL
    );

    static final ISeq<Op<Double>> TERMINALS = ISeq.of(
            Var.of("x", 0),
            EphemeralConst.of(() ->
                    (double) RandomRegistry.getRandom().nextInt(10))
    );
    // The lookup table where the data points are stored.
    static final double[][] SAMPLES = new double[][]{
            {0, 0.000},
            {0.10, 0.0740},
            {0.20, 0.1120},
            {0.30, 0.1380},
            {0.40, 0.176},
            {0.5, 0.25},
            {0.6, 0.384},
            {0.7, 0.602},
            {0.8, 0.928},
            {0.9, 1.386},
            {1.0, 2.00},
            {1.1, 2.794},
            {1.2, 3.792},
            {1.3, 5.018},
            {1.4, 6.496},
            {1.5, 8.25},
            {1.6, 10.304},
            {1.7, 12.682},
            {1.8, 15.482},
            {1.9, 18.506},
            {2.00, 22.00}
    };

    static double error(final ProgramGene<Double> program) {
        return Arrays.stream(SAMPLES).mapToDouble(sample -> {
            final double x = sample[0];
            final double y = sample[1];
            final double result = program.eval(x);
            return abs(y - result) + program.size() * 0.0001;
        })
                .sum();
    }
    static final Codec<ProgramGene<Double>, ProgramGene<Double>> CODEC =
            Codec.of(
                    Genotype.of(ProgramChromosome.of(
                            // Program tree depth.
                            5,
                            // Chromosome validator.
                            ch -> ch.getRoot().size() <= 50,
                            OPERATIONS,
                            TERMINALS
                    )),
                    Genotype::getGene
            );

    public static void main(String[] args) {
        final Engine<ProgramGene<Double>, Double> engine = Engine
                .builder(ProgramTest::error, CODEC)
                .minimizing()
                .alterers(
                        new SingleNodeCrossover<>(),
                        new Mutator<>())
                .build();

        final ProgramGene<Double> program = engine.stream()
                .limit(50000)
                .collect(EvolutionResult.toBestGenotype())
                .getGene();

        System.out.println(Tree.toString(program));
    }

}
