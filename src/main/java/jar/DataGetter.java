package jar;

import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Random;
import java.util.function.Supplier;
import jar.DBAccess;
import sun.reflect.generics.reflectiveObjects.NotImplementedException;

/**
 * Data Getter is responsible for the generation of individuals
 *
 * It is in the data getter which we access data from a DBAccess class and perform the random permutations along the
 *
 */
public class DataGetter {
    private static Random rand = new Random();
    private static String[] columns;
    private static ResultSet resCache;
    private static String makeCol(int column) {
        if (columns == null) {
            Statement st;
            try {
                st = DBAccess.getConnection().createStatement();
                ResultSet rs = st.executeQuery("SELECT * FROM " + Util.tablename + " LIMIT 1;");
                resCache = rs;

                ResultSetMetaData rsmd = rs.getMetaData();
                int cols = rsmd.getColumnCount();
                columns = new String[cols];

                for (int i = 0; i < cols; i++) {
                    columns[i] = rsmd.getColumnName(i+1);
                }
            } catch (SQLException e) {
                e.printStackTrace();

            } catch (ClassNotFoundException e) {
                e.printStackTrace();
            }
        }
        return columns[column];
    }

    private static Integer rows;
    private static int getNumRows() throws SQLException, ClassNotFoundException {
        if (rows == null) {
            Statement st = DBAccess.getConnection().createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE, ResultSet.CONCUR_READ_ONLY);
            ResultSet rs = st.executeQuery("SELECT * FROM " + Util.tablename + ";");

            //Would get the number of rows in resultset
            rs.last();
            rows = rs.getRow();
        }
        return rows;
    }

    private static Integer cols;
    private static int getNumCols() throws SQLException, ClassNotFoundException {
        if (columns == null) {
            Statement st;
            try {
                st = DBAccess.getConnection().createStatement();
                ResultSet rs = st.executeQuery("SELECT * FROM " + Util.tablename + " LIMIT 1;");
                resCache = rs;

                ResultSetMetaData rsmd = rs.getMetaData();
                int cols = rsmd.getColumnCount();
                columns = new String[cols];

                for (int i = 0; i < cols; i++) {
                    columns[i] = rsmd.getColumnName(i+1);
                }
            } catch (SQLException e) {
                e.printStackTrace();

            } catch (ClassNotFoundException e) {
                e.printStackTrace();
            }
        }
        return columns.length;
    }

    private static String makeData (String column, int row) throws SQLException, ClassNotFoundException {

        Statement st = DBAccess.getConnection().createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE, ResultSet.CONCUR_READ_ONLY);
        ResultSet rs = st.executeQuery("SELECT " + column + " FROM " + Util.tablename + ";");
        rs.relative(row);

        //Should always be the 1st column, since we only have one column in the select
        return rs.getString(1);

    }
    private static String makeComparator() {
        String[] ops = new String[]{">", "<", "!=", "=="};
        return ops[rand.nextInt(ops.length - 1)];
    }

    public static String makePredicate() throws SQLException, ClassNotFoundException {
        int col = rand.nextInt(getNumCols() - 1) + 1;
        int row = rand.nextInt(getNumRows() - 1) + 1;
        String colName = makeCol(col);
        return "'" + colName + "'" + makeComparator() + "'" + makeData(colName, row) + "'";
    }

    public static Supplier predicateSupplier = () -> {
        try {
            return makePredicate();
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } return null;
    };
}
