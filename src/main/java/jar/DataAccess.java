package database;

import java.sql.*;
import java.util.TimeZone;

import scores.Utils;
import exceptions.DatabaseException;

public class DataAccess 
{
    private static DataAccess singleton = null;
    /*
     * DataAccess deals with one connection only. If you need more than one connection,
     * you have to make more DataAccess objects.
     */
    private Connection connection;
   // private String url = "jdbc:mysql://50.62.209.12:3306/archery";
    private static String url = "jdbc:mysql://"+Utils.properties.getProperty("database_host")+"/"+
	    Utils.properties.getProperty("database_name")+"?useLegacyDatetimeCode=false&serverTimezone="
	    +TimeZone.getDefault().getID();
    private static String dbPassword = null;

    
    protected DataAccess(){};

    public Connection getConnection() throws DatabaseException  
    {
	try
	{
	    if(connection == null || connection.isClosed())
	    {
		connection = makeConnection();

	    }
	}catch (SQLException e)
	{
	    throw new DatabaseException("Connection problems: "+e.getMessage());
	} 
	return connection;
    }


    protected Connection makeConnection() throws DatabaseException
    {
	Connection connection = null;
	
	try
	{
	    Class.forName("com.mysql.jdbc.Driver");
	    connection = DriverManager.getConnection( url, Utils.properties.getProperty("db_user_name"), 
		dbPassword);
	} catch (ClassNotFoundException e)
	{
	    throw new DatabaseException("No suitable jdbc driver found: "+e.getMessage());
	}catch (SQLException e)
	{
	    throw new DatabaseException("Connection failed: "+e.getMessage());
	}
	return connection;


    }
    
    public static DataAccess getDataAccess()
    {
	if (singleton == null)
	    singleton = new DataAccess();
	return singleton;
    }
    
    public static void setDbPassword(String pwd) throws DatabaseException
    {
	Connection conn;
	try
	{
	    conn = DriverManager.getConnection( url, Utils.properties.getProperty("db_user_name"), 
		    pwd);
	    if(!conn.isValid(10))
		throw new DatabaseException("Password is wrong");
	    dbPassword = pwd;
	} catch (SQLException e)
	{
	    throw new DatabaseException(e.getMessage());
	}

		
    }
    
    public static boolean hasPassword()
    {
	if(dbPassword == null)
	    return false;
	return true;
    }

}
