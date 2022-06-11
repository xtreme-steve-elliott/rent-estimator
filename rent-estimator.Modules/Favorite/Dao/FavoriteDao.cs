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
            model.id,
            model.accountId,
            model.propertyId
        };
        return await _dbConnection.QueryFirstAsync<FavoriteModel>(_favoriteSql.CreateFavoriteSql(), param);
    }

    public async Task<IEnumerable<FavoriteModel>> GetFavorites(string accountId)
    {
        var param = new { accountId };
        return await _dbConnection.QueryAsync<FavoriteModel>(_favoriteSql.GetFavoritesSql(), param);
    }
}

public interface IFavoriteDao
{
    Task<FavoriteModel> CreateFavorite(FavoriteModel model);
    Task<IEnumerable<FavoriteModel>> GetFavorites(string accountId);
}