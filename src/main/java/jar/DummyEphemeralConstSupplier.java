package jar;

import java.util.Random;
import java.util.function.Supplier;

/**
 * Dummy implementation of the ephemeral const supplier. Eventually these will take two args which will be randomly
 * chosen to be the column and value
 *
 * The supplier should provide a value in the get method which is applicable to the current column being investigated...
 * I'm not sure that the existing infractructure actually supports this and we may in fact need to extend the OP interface
 * to account for this. If extending OP doesn't work we may need to extend the
 */

public class DummyEphemeralConstSupplier implements Supplier<String> {
    private static String[] colArray;
    private static String[][] dataArray;
    private static Random rand = new Random();

    public void AddVals(String[] col, String[][] data) {
        colArray = col;
        dataArray = data;
    }
    public String get() {
        // Get the two dimensions of the array
        int colIndex = rand.nextInt(colArray.length);
        int dataIndex = rand.nextInt(dataArray[0].length);

        return colArray[colIndex] + ">" + dataArray[colIndex][dataIndex];
    }
}
