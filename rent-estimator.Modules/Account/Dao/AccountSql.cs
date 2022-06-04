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
                                 [CreatedAt]
                                 [LastUpdatedAt]
                                )
                                OUTPUT inserted.Id, inserted.FirstName, inserted.LastName, inserted.Username, inserted.Password, inserted.CreatedAt, inserted.LastUpdatedAt
                                VALUES 
                                (@Id, 
                                 @FirstName, 
                                 @LastName, 
                                 @Username,
                                 @Password
                                 @CreatedAt
                                 @LastUpdatedAt
                                )";
    }
}

public interface IAccountSql
{
    string CreateAccountSql();
}