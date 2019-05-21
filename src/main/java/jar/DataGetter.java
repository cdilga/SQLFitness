package jar;

import java.util.Random;
import java.util.function.Supplier;
import jar.DBAccess;
/**
 * Data Getter is responsible for the generation of individuals
 *
 * It is in the data getter which we access data from a DBAccess class and perform the random permutations along the
 *
 */
public class DataGetter {
    private static Random rand = new Random();
    private static String[][] data = new String[][]{
            {"-25.676187", "134.051234", "Australia", "M", "James L Hamburger", "2", "Star"},
            {"-43.225193", "170.588218", "New Zealand ", "F", "Jonothan P. Prince", "34", "Hash"},
            {"-2.322972", "115.417038", "Indonesia", "M", "Xavier et al.", "5", "Circle"},
            {"4.00126", "102.766321", "Malaysia", "F", "Gordon Stevenson", "4", "Eeel"},
            {"-37.050793", "147.042446", "Australia ", "M", "Ron K. Kelly", "Gareth J, Sherman", "3", "Square"}
    };

    private static String[] columns = new String[]{"lat","lon","Country","Sex","Author","Length","Symbol"};

    private static String makeCol(int column) {

        return columns[column];
    }
    private static String makeData (int column, int row) {
        return data[column][row];
    }
    private static String makeComparator() { return ">"; }

    public static String makePredicate() {
        int col = 1;
        int row = 1;
        return makeCol(col) + makeComparator() + makeData(col, row);
    }

    public static Supplier predicateSupplier = () -> makePredicate();
}
