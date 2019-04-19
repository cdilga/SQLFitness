package jar;

import java.util.function.Supplier;

/**
 * Dummy implementation of the ephemeral const supplier. Eventually these will take two args which will be randomly
 * chosen to be the column and value
 *
 * The supplier should provide a value in the get method which is applicable to the current column being investigated...
 * I'm not sure that the existing infractructure actually supports this and we may infact need to extend the OP interface
 * to account for this. If extending OP doesn't work we may need to extend the
 */
public class DummyEphemeralConstSupplier implements Supplier<String> {
    @Override Supplier<String> (column, String value) {

    }
    public String get() {
        return "A > 5";
    }
}
