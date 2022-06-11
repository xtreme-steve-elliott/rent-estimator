using System;
using System.Collections.Generic;
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
    public async void FavoriteDao_WhenCreateFavoriteQueryRuns_InvokesDbConnection()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        const string propertyId = "M7952539079";
        var favoriteModel = new FavoriteModel
        {
            id = favoriteId,
            accountId = accountId,
            propertyId = propertyId
        }; 
        var param = new 
        {
            id = favoriteId,
            accountId,
            propertyId
        };
        var query = _favoriteSql.CreateFavoriteSql();

        //act
        await _favoriteDao.CreateFavorite(favoriteModel);

        //assert
        _db.Verify(db => db.QueryFirstAsync<FavoriteModel>(query, It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p))), Times.Once);
    }
    
    [Fact]
    public async void FavoriteDao_CreateAccount_SavesAndReturnsCreatedAccount()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        const string propertyId = "M7952539079";
        var favoriteModel = new FavoriteModel
        {
            id = favoriteId,
            accountId = accountId,
            propertyId = propertyId
        }; 
        var param = new 
        {
            id = favoriteId,
            accountId,
            propertyId
        };
        var query = _favoriteSql.CreateFavoriteSql();
        _db.Setup(db => db.QueryFirstAsync<FavoriteModel>(query, It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p)))).ReturnsAsync(favoriteModel);

        //act
        var savedModel = await _favoriteDao.CreateFavorite(favoriteModel);

        //assert
        savedModel.Should().BeEquivalentTo(favoriteModel);
    }

    [Fact]
    public async void FavoriteDao_WhenGetFavoritesQueryRuns_InvokesDbConnection()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var param = new { accountId };
        var query = _favoriteSql.GetFavoritesSql();

        //act
        await _favoriteDao.GetFavorites(accountId);

        //assert
        _db.Verify(db => db.QueryAsync<FavoriteModel>(query, It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p))), Times.Once);
    }
    
    [Fact]
    public async void FavoriteDao_GetFavorites_ReturnsListOfFavoritesForGivenAccount()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        const string propertyId = "M7952539079";
        var favoriteModel = new FavoriteModel
        {
            id = favoriteId,
            accountId = accountId,
            propertyId = propertyId
        };
        var models = new List<FavoriteModel>{ favoriteModel };
        var param = new { accountId };
        var query = _favoriteSql.GetFavoritesSql();
        _db.Setup(db => db.QueryAsync<FavoriteModel>(query, It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p)))).ReturnsAsync(models);

        //act
        var fetchedModels = await _favoriteDao.GetFavorites(accountId);

        //assert
        fetchedModels.Should().BeEquivalentTo(models);
    }
}