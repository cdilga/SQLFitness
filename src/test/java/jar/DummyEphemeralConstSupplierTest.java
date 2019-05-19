package jar;

import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import static org.junit.Assert.*;

public class DummyEphemeralConstSupplierTest {
    private String[][] data;
    private String[] columns;

    @BeforeClass
    public void setUp() {
        data = new String[][]{
                {"-25.676187","134.051234","Australia" ,"M","James L Hamburger","2","Star"},
                {"-43.225193","170.588218","New Zealand ","F","Jonothan P. Prince","34","Hash"},
                {"-2.322972","115.417038","Indonesia","M","Xavier et al.","5","Circle"},
                {"4.00126","102.766321","Malaysia","F","Gordon Stevenson","4","Eeel"},
                {"-37.050793","147.042446","Australia ","M","Ron K. Kelly", "Gareth J, Sherman","3","Square"}
        };
        columns = new String[]{"lat","lon","Country","Sex","Author","Length","Symbol"};
    }

    @Test
    public void getColumnns() {
        DummyEphemeralConstSupplier dum = new DummyEphemeralConstSupplier(columns, data);

        String[] result = dum.get().split("[<>]|(!=)|(==)|(<>)");
        Boolean contained = false;
        for (int i = 0; i < columns.length; i++) {
            if (columns[i] == result[0]) {
                contained = true;
                break;
            }
        }

        assertTrue(contained);
    }

    @Test
    public void getData() {
        DummyEphemeralConstSupplier dum = new DummyEphemeralConstSupplier(columns, data);
        String[] result = dum.get().split("[<>]|(!=)|(==)|(<>)");
        assertTrue(result.length == 2);
        for (int i = 0; i < data.length; i++) {
            for (int j = 0; j < data[i].length; j++) {
                assertTrue(false);
            }
        }
    }

}