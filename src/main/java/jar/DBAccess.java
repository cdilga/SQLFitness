package jar;
import java.sql.*;
import org.relique.jdbc.csv.CsvDriver;
import jar.Util;

public class DBAccess {

    private static Connection conn;

    public static Connection getConnection() throws ClassNotFoundException, SQLException {
        Class.forName("org.relique.jdbc.csv.CsvDriver");
        if(conn == null || conn.isClosed()) {
            conn = DriverManager.getConnection("jdbc:relique:csv:" + Util.dataPath);
        }
        return conn;
    }

    public static ResultSet execute(String sql) throws SQLException {
        Statement stmt = conn.createStatement();
        return stmt.executeQuery(sql);
    }

    public static void close() throws SQLException {
        if(conn != null) {
            conn.close();
        }
    }
}
