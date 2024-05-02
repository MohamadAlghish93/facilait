using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using FacilaIT.Helper.Shared;
using Microsoft.Extensions.Caching.Memory;
using FacilaIT.Models;
using FacilaIT.Helper.Shared;


namespace FacilaIT.Helper;
public class DatabaseWrapper
{
    private readonly string _oracleConnectionString;
    private readonly string _sqlConnectionString;

    public DatabaseWrapper(string oracleConnectionString, string sqlConnectionString)
    {
        _oracleConnectionString = oracleConnectionString;
        _sqlConnectionString = sqlConnectionString;
    }

    public DatabaseWrapper()
    {
        string cacheKey = Constant.Cache_General_AppSettings;
        SettingItem cachedData;
        if (!CacheManager.Cache.TryGetValue(cacheKey, out cachedData))
        {
            string error = $"Error: on get geneal settings";
            Exception e = new Exception(error);
            throw e;
        }
        else
        {
            _oracleConnectionString = cachedData.ConnectionStringOra.Replace(Constant.Password_Deleimter, PasswordEncryptor.DecryptString(cachedData.OraPassword));
            _sqlConnectionString = cachedData.ConnectionStringSql.Replace(Constant.Password_Deleimter, PasswordEncryptor.DecryptString(cachedData.SQLPassword)); ;
        }
    }

    public List<T> QuerySelect<T>(int typeDB, string sql, object parameters = null)
    {
        switch (typeDB)
        {
            case (int)Common.enumDatabaseType.SQL: // SQL
                return QuerySql<T>(sql, parameters);
            case (int)Common.enumDatabaseType.Oracle: // Oracle
                return QueryOracle<T>(sql, parameters);
        }

        return new List<T>();
    }

    public List<T> QueryOracle<T>(string sql, object parameters = null)
    {
        try
        {
            using var connection = new OracleConnection(_oracleConnectionString);
            return connection.Query<T>(sql, parameters).ToList();
        }
        catch (OracleException ex)
        {
            // Handle the Oracle exception
            Console.WriteLine($"Oracle exception occurred: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Generic exception occurred: {ex.Message}");
            // Log the exception or rethrow it if necessary
            throw;
        }
    }

    public List<T> QuerySql<T>(string sql, object parameters = null)
    {
        try
        {
            using var connection = new SqlConnection(_sqlConnectionString);
            return connection.Query<T>(sql, parameters).ToList();
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            Console.WriteLine($"SQL exception occurred: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Generic exception occurred: {ex.Message}");
            // Log the exception or rethrow it if necessary
            throw;
        }
    }

    public int ExecuteOracle(string sql, object parameters = null)
    {
        using var connection = new OracleConnection(_oracleConnectionString);
        return connection.Execute(sql, parameters);

    }

    public int ExecuteSql(string sql, object parameters = null)
    {
        using var connection = new SqlConnection(_sqlConnectionString);
        return connection.Execute(sql, parameters);
    }
}
