namespace rent_estimator.Modules.Favorite.Dao;

public class FavoriteSql: IFavoriteSql
{
    public FavoriteSql() {}
 
    public string CreateFavoriteSql()
    {
        return @"INSERT INTO [dbo].[Favorite]
                                ([Id], 
                                 [AccountId]
                                )
                                OUTPUT inserted.Id, inserted.AccountId
                                VALUES 
                                (@Id, 
                                 @AccountId
                                )";
    }
}

public interface IFavoriteSql
{
    string CreateFavoriteSql();
}