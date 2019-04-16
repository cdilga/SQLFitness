package jar;

import java.util.function.Supplier;

/**
 * Dummy implementation of the ephemeral const supplier. Eventually these will take two args which will be randomly
 * chosen to be the column and value
 */
public class DummyEphemeralConstSupplier implements Supplier<String> {
    @Override Supplier<String> (column, String value) {

    }
    public String get() {
        return "A > 5";
    }
}
