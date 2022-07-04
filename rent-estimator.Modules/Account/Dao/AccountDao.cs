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
    
    public Task<AccountModel> CreateAccount(AccountModel account)
    {
        // TODO: I don't like the fact insertion is done through a query. It's not single responsibility enough
        return _dbConnection.QueryFirstAsync<AccountModel>(_accountSql.CreateAccountSql(), account);
    }
}

public interface IAccountDao
{
    Task<AccountModel> CreateAccount(AccountModel account);
}
