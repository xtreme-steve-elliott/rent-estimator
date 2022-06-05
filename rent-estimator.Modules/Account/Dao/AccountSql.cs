namespace rent_estimator.Modules.Account.Dao;

public class AccountSql : IAccountSql
{
    public AccountSql() {}
 
    public string CreateAccountSql()
    {
        return @"INSERT INTO [dbo].[Account]
                                ([Id], 
                                 [FirstName], 
                                 [LastName], 
                                 [Username],
                                 [Password]
                                )
                                OUTPUT inserted.Id, inserted.FirstName, inserted.LastName, inserted.Username, inserted.Password
                                VALUES 
                                (@Id, 
                                 @FirstName, 
                                 @LastName, 
                                 @Username,
                                 @Password
                                )";
    }
}

public interface IAccountSql
{
    string CreateAccountSql();
}