namespace rent_estimator.Modules.Favorite.Dao;

public class FavoriteSql: IFavoriteSql
{
    public FavoriteSql() {}
 
    public string CreateFavoriteSql()
    {
        return @"INSERT INTO [dbo].[Favorite]
                                ([Id], 
                                 [AccountId],
                                 [PropertyId]
                                )
                                OUTPUT inserted.Id, inserted.AccountId, inserted.PropertyId
                                VALUES 
                                (@Id, 
                                 @AccountId,
                                 @PropertyId
                                )";
    }
}

public interface IFavoriteSql
{
    string CreateFavoriteSql();
}