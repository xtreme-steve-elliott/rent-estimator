namespace rent_estimator.Modules.Account.Dao;

public class AccountSql : IAccountSql
{
    public string CreateAccountSql()
    {
       return
            $@"
            INSERT INTO [dbo].[Account]
            (
                [Id],
                [FirstName],
                [LastName],
                [Username],
                [Password]
            )
            OUTPUT
                inserted.Id AS {nameof(AccountModel.Id)},
                inserted.FirstName AS {nameof(AccountModel.FirstName)},
                inserted.LastName AS {nameof(AccountModel.LastName)},
                inserted.Username AS {nameof(AccountModel.Username)},
                inserted.Password AS {nameof(AccountModel.Password)}
            VALUES 
            (
                @{nameof(AccountModel.Id)}, 
                @{nameof(AccountModel.FirstName)}, 
                @{nameof(AccountModel.LastName)}, 
                @{nameof(AccountModel.Username)},
                @{nameof(AccountModel.Password)}
            )";
    }
}

public interface IAccountSql
{
    string CreateAccountSql();
}
