package jar;

import java.util.Random;

//Lets chuck all of the db logic and everything that needs to access the db in here, and just let everything access it
//See what happens
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

    private static String makeCol() {
        return "col";
    }
    private static String makeData (int column) {
        return "data";
    }
    private static String

    public static String makePredicate() {

        return
    }
}
