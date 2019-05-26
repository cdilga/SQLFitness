package jar;

import io.jenetics.Genotype;
import io.jenetics.engine.Codec;
import io.jenetics.prog.ProgramChromosome;
import io.jenetics.prog.ProgramGene;
import io.jenetics.prog.op.EphemeralConst;
import io.jenetics.prog.op.Op;
import io.jenetics.prog.op.Program;
import io.jenetics.util.ISeq;
import org.bytedeco.javacpp.presets.opencv_core;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Arrays;

import static org.junit.Assert.*;

public class FitnessFunctionTest {
    private static FitnessFunction fitness;
    private static String[][] data;
    private static String[] columns;


    @Before
    public void setUp() throws Exception {

        data = new String[][]{
                {"-25.676187","134.051234","Australia" ,"M","James L Hamburger","2","Star"},
                {"-43.225193","170.588218","New Zealand ","F","Jonothan P. Prince","34","Hash"},
                {"-2.322972","115.417038","Indonesia","M","Xavier et al.","5","Circle"},
                {"4.00126","102.766321","Malaysia","F","Gordon Stevenson","4","Eeel"},
                {"-37.050793","147.042446","Australia ","M","Ron K. Kelly", "Gareth J, Sherman","3","Square"}
        };
        columns = new String[]{"lat","lon","Country","Sex","Author","Length","Symbol"};
        ArrayList<String> arrCols = (ArrayList<String>) Arrays.asList(columns);
        fitness = new FitnessFunction(arrCols);
    }

    @Test
    public void testFitnessSQL() {
        //In order to test the sql is valid we need to pass in a program gene mock or stub, which really just needs to
        // provide .eval() and return the string we saw earlier from the DataGetter
        //
        //assertEquals(0.0, fitness.fitness());
    }

}