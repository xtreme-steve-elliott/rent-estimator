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

    public Task<FavoriteModel> CreateFavorite(FavoriteModel model)
    {
        return _dbConnection.QueryFirstAsync<FavoriteModel>(_favoriteSql.CreateFavoriteSql(), model);
    }

    public Task<IEnumerable<FavoriteModel>> GetFavorites(string accountId)
    {
        var param = new
        {
            AccountId = accountId
        };
        return _dbConnection.QueryAsync<FavoriteModel>(_favoriteSql.GetFavoritesSql(), param);
    }
}

public interface IFavoriteDao
{
    Task<FavoriteModel> CreateFavorite(FavoriteModel model);
    Task<IEnumerable<FavoriteModel>> GetFavorites(string accountId);
}
