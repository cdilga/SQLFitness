package jar;

import io.jenetics.BitChromosome;
import io.jenetics.BitGene;
import io.jenetics.Genotype;
import io.jenetics.engine.Engine;
import io.jenetics.engine.EvolutionResult;
import io.jenetics.util.Factory;

public class GensimTest {
    // 2.) Definition of the fitness function.
    private static int eval(Genotype<BitGene> gt) {
        return gt.getChromosome()
                .as(BitChromosome.class)
                .bitCount();
    }
    //Broadly we need to take a bunch of column names and use them to select columns
    //The fitness function will need something like: column names - question???
    //The fitness function needs to be damn lightweight. Probably also needs to be threadsafe and stuff like this
    //the fitness function needs access to the word vectors in memory
    //This class will then import from the other classes...

    String cols = "AFD_higher_taxon,Order,Genus,Subgenus,Species,Subspecies,Name_type,Rank,Orig_combination,Author,Year,Full_author_name,No_auths,A_in_B,Pub_ID,Citation,Pub_type,Pub_country,Pub_ento,J_title,A_publr_class,A_publr";
    public static void main(String[] args) {
        // 1.) Define the genotype (factory) suitable
        //     for the problem.
        Factory<Genotype<BitGene>> gtf =
                Genotype.of(BitChromosome.of(10, 0.5));

        // 3.) Create the execution environment.
        Engine<BitGene, Integer> engine = Engine
                .builder(GensimTest::eval, gtf)
                .build();

        // 4.) Start the execution (evolution) and
        //     collect the result.
        Genotype<BitGene> result = engine.stream()
                .limit(100)
                .collect(EvolutionResult.toBestGenotype());

        System.out.println("Hello World:\n" + result);
    }
}
