namespace rent_estimator.Modules.Favorite.Dao;

public class FavoriteSql: IFavoriteSql
{
    public string CreateFavoriteSql()
    {
        return
            $@"
            INSERT INTO [dbo].[Favorite]
            (
                [Id], 
                [AccountId],
                [PropertyId]
            )
            OUTPUT
                inserted.Id AS {nameof(FavoriteModel.Id)},
                inserted.AccountId AS {nameof(FavoriteModel.AccountId)},
                inserted.PropertyId AS {nameof(FavoriteModel.PropertyId)}
            VALUES 
            (
                @{nameof(FavoriteModel.Id)}, 
                @{nameof(FavoriteModel.AccountId)},
                @{nameof(FavoriteModel.PropertyId)}
            )";
    }

    public string GetFavoritesSql()
    {
        return @"SELECT * FROM [dbo].[Favorite] WHERE AccountId = @AccountId";
    }
}

public interface IFavoriteSql
{
    string CreateFavoriteSql();
    string GetFavoritesSql();
}
