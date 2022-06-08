using System;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Shared.Dapper;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Dao;

public class FavoriteDaoTests
{
    private readonly Mock<IDapperWrapper> _db;
    private readonly IFavoriteDao _favoriteDao;
    private readonly FavoriteSql _favoriteSql;

    public FavoriteDaoTests()
    {
        _favoriteSql = new FavoriteSql();
        _db = new Mock<IDapperWrapper>();
        _favoriteDao = new FavoriteDao(_db.Object, _favoriteSql);
    }

    [Fact]
    public void FavoriteDao_ImplementsIFavoriteDaoInterface()
    {
        _favoriteDao.Should().BeAssignableTo<IFavoriteDao>();
    }

    [Fact]
    public async void AccountDao_WhenQueryRuns_InvokesDbConnection()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        var favoriteModel = new FavoriteModel
        {
            id = favoriteId,
            accountId = accountId
        }; 
        var param = new 
        {
            id = favoriteId,
            accountId
        };
        var query = _favoriteSql.CreateFavoriteSql();

        //act
        await _favoriteDao.CreateFavorite(favoriteModel);

        //assert
        _db.Verify(db => db.QueryFirstAsync<FavoriteModel>(query, It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p))), Times.Once);
    }
    
    [Fact]
    public async void AccountDao_CreateAccount_SavesAndReturnsCreatedAccount()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        var favoriteModel = new FavoriteModel
        {
            id = favoriteId,
            accountId = accountId
        }; 
        var param = new 
        {
            id = favoriteId,
            accountId = accountId
        };
        var query = _favoriteSql.CreateFavoriteSql();
        _db.Setup(db => db.QueryFirstAsync<FavoriteModel>(query, It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p)))).ReturnsAsync(favoriteModel);

        //act
        var savedModel = await _favoriteDao.CreateFavorite(favoriteModel);

        //assert
        savedModel.Should().BeEquivalentTo(favoriteModel);
    }
}