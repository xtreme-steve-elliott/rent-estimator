using rent_estimator.Shared.Dapper;

namespace rent_estimator.Modules.Favorite.Dao;

public class FavoriteDao : IFavoriteDao
{
    private readonly IDapperWrapper _dbConnection;
    private readonly IFavoriteSql _favoriteSql;
        
    public FavoriteDao(IDapperWrapper dbConnection, IFavoriteSql sql)
    {
        _dbConnection = dbConnection;
        _favoriteSql = sql;
    }

    public async Task<FavoriteModel> CreateFavorite(FavoriteModel model)
    {
        var param = new
        {
            id = model.id,
            accountId = model.accountId
        };
        return await _dbConnection.QueryFirstAsync<FavoriteModel>(_favoriteSql.CreateFavoriteSql(), param);
    }
}

public interface IFavoriteDao
{
    Task<FavoriteModel> CreateFavorite(FavoriteModel model);
}