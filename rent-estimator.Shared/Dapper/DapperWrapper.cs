using System.Data;
using Dapper;

namespace rent_estimator.Shared.Dapper;

public interface IDapperWrapper
{
    public Task<IEnumerable<T>> QueryAsync<T>(String query);
    public Task<T> QueryFirstAsync<T>(String query, object param);
}

public class DapperWrapper : IDapperWrapper
{
    private readonly IDbConnection _dbConnection;

    public DapperWrapper(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string query)
    {
        return await _dbConnection.QueryAsync<T>(query, null, null, null, CommandType.Text);
    }
    
    public async Task<T> QueryFirstAsync<T>(string query, object param)
    {
        return await _dbConnection.QueryFirstAsync<T>(query, param, null, null, CommandType.Text);
    }
}