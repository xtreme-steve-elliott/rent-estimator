using rent_estimator.Shared.Dapper;

namespace rent_estimator.Modules.Account.Dao;

public class AccountDao : IAccountDao
{
    private readonly IDapperWrapper _dbConnection;
    private readonly IAccountSql _accountSql;

    public AccountDao(IDapperWrapper dbConnection, IAccountSql accountSql)
    {
        _dbConnection = dbConnection;
        _accountSql = accountSql;
    }
    
    public async Task<AccountModel> CreateAccount(AccountModel account)
    {
        var sql = _accountSql.CreateAccountSql();
        var param = new
        {
            Id = account.Id,
            Username =account.Username,
            Password =account.Password,
            FirstName = account.FirstName,
            LastName = account.LastName
        };
        return await _dbConnection.QueryFirstAsync<AccountModel>(sql, param);
    }
}

public interface IAccountDao
{
    Task<AccountModel> CreateAccount(AccountModel account);
}