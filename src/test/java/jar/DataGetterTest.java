package jar;

import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import java.sql.SQLException;

import static org.junit.Assert.*;

public class DataGetterTest {
    private static String[][] data;
    private static String[] columns;

    @BeforeClass
    public static void setUp() {
        data = new String[][]{
                {"-25.676187","134.051234","Australia" ,"M","James L Hamburger","2","Star"},
                {"-43.225193","170.588218","New Zealand ","F","Jonothan P. Prince","34","Hash"},
                {"-2.322972","115.417038","Indonesia","M","Xavier et al.","5","Circle"},
                {"4.00126","102.766321","Malaysia","F","Gordon Stevenson","4","Eeel"},
                {"-37.050793","147.042446","Australia ","M","Ron K. Kelly", "Gareth J, Sherman","3","Square"}
        };
        columns = new String[]{"lat","lon","Country","Sex","Author","Length","Symbol"};
    }

    /**
     * Tests the format of predicate is indeed valid
     *
     * Primitive and doesn't check for special chars or other encoding issues that may arise
     */
    @Test
    public void isPredicate() {
        String pred;
        try {
            pred = DataGetter.makePredicate();
            String[] result = pred.split("[<>]|(!=)|(==)|(<>)");
            assertEquals(2, result.length);
        } catch (SQLException e) {
            e.printStackTrace();
            assertTrue(false);
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
            assertTrue(false);
        }



    }

    /**
     * Tests that there is indeed a column within the range of the data returned
     *
     * Equates to left hand side of the expression
     */
    @Test
    public void getColumnns() {
        String pred = null;
        try {
            pred = DataGetter.makePredicate();
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        String[] result = pred.split("[<>]|(!=)|(==)|(<>)");
        Boolean contained = false;
        for (int i = 0; i < columns.length; i++) {
            if (columns[i].equals(result[0])) {
                contained = true;
                break;
            }
        }

        assertTrue(contained);
    }


    /**
     * Tests the data is returned from somewhere in the data source
     *
     * Equates to testing the right hand side of the expression
     */
    @Test
    public void getData() {
        String pred = null;
        try {
            pred = DataGetter.makePredicate();
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        //Note that the order of this regex is rather important
        String[] result = pred.split("(!=)|(==)|[<>]");
        assertTrue(result.length == 2);

        Boolean contained = false;
        for (int i = 0; i < data.length; i++) {
            for (int j = 0; j < data[i].length; j++) {
                if (data[i][j].equals(result[1])) {
                    contained = true;
                    break;
                }
            }
        }
        assertTrue(contained);
    }

}