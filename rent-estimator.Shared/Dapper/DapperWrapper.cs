using System.Data;
using Dapper;

namespace rent_estimator.Shared.Dapper;

public interface IDapperWrapper
{
    Task<IEnumerable<T>> QueryAsync<T>(string query, object param);
    Task<T> QueryFirstAsync<T>(string query, object param);
}

public class DapperWrapper : IDapperWrapper
{
    private readonly IDbConnection _dbConnection;

    public DapperWrapper(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<IEnumerable<T>> QueryAsync<T>(string query, object param)
    {
        return _dbConnection.QueryAsync<T>(query, param, null, null, CommandType.Text);
    }
    
    public Task<T> QueryFirstAsync<T>(string query, object param)
    {
        return _dbConnection.QueryFirstAsync<T>(query, param, null, null, CommandType.Text);
    }
}
