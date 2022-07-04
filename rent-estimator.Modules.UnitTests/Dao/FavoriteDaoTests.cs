using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Shared.Dapper;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Dao;

public class FavoriteDaoTests
{
    private const string CreateSqlStatement = "create test";
    private const string GetSqlStatement = "get test";
    private readonly Mock<IDapperWrapper> _dbMock;
    private readonly Mock<IFavoriteSql> _favoriteSqlMock;
    private readonly IFavoriteDao _favoriteDao;

    public FavoriteDaoTests()
    {
        _dbMock = new Mock<IDapperWrapper>();
        _favoriteSqlMock = new Mock<IFavoriteSql>();
        _favoriteSqlMock
            .Setup(_ => _.CreateFavoriteSql())
            .Returns(CreateSqlStatement);
        _favoriteSqlMock
            .Setup(_ => _.GetFavoritesSql())
            .Returns(GetSqlStatement);
        _favoriteDao = new FavoriteDao(_dbMock.Object, _favoriteSqlMock.Object);
    }

    [Fact]
    public async Task CreateFavorite_ShouldCall_FavoriteSqlCreateFavoriteSql_AndDbQueryFirstAsync()
    {
        //arrange
        var favoriteModel = new FavoriteModel
        {
            Id = Guid.NewGuid().ToString(),
            AccountId = Guid.NewGuid().ToString(),
            PropertyId = "M7952539079"
        };

        //act
        await _favoriteDao.CreateFavorite(favoriteModel);

        //assert
        _favoriteSqlMock.Verify(_ => _.CreateFavoriteSql(), Times.Once);
        _dbMock.Verify(_ => _.QueryFirstAsync<FavoriteModel>(CreateSqlStatement, favoriteModel), Times.Once);
    }
    
    [Fact]
    public async Task CreateFavorite_ShouldReturn_CreatedFavorite()
    {
        //arrange
        var favoriteModel = new FavoriteModel
        {
            Id = Guid.NewGuid().ToString(),
            AccountId = Guid.NewGuid().ToString(),
            PropertyId = "M7952539079"
        };
        
        _dbMock
            .Setup(_ => _.QueryFirstAsync<FavoriteModel>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(favoriteModel);

        //act
        var createdFavorite = await _favoriteDao.CreateFavorite(favoriteModel);

        //assert
        createdFavorite.Should().BeEquivalentTo(favoriteModel);
    }

    [Fact]
    public async Task GetFavourites_ShouldCall_FavoriteSqlGetFavoritesSql_AndDbQueryAsync()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var expectedParam = new
        {
            AccountId = accountId
        };
        var capturedParam = new object();

        _dbMock
            .Setup(_ => _.QueryAsync<FavoriteModel>(It.IsAny<string>(), It.IsAny<object>()))
            .Callback<string, object>((_, o) =>
            {
                capturedParam = o;
            });

        //act
        await _favoriteDao.GetFavorites(accountId);

        //assert
        capturedParam.Should().BeEquivalentTo(expectedParam);
        _favoriteSqlMock.Verify(_ => _.GetFavoritesSql(), Times.Once);
        _dbMock.Verify(_ => _.QueryAsync<FavoriteModel>(GetSqlStatement, capturedParam), Times.Once);
    }
    
    [Fact]
    public async Task GetFavorites_ShouldReturn_IEnumerableOfFavoriteModels_ForRequestedAccountId()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteModels = new List<FavoriteModel>
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = accountId,
                PropertyId = "M7952539079"
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = accountId,
                PropertyId = "M7952539080"
            }
        };
        var expectedFavoriteModels = favoriteModels;
        _dbMock
            .Setup(_ => _.QueryAsync<FavoriteModel>(GetSqlStatement, It.IsAny<object>()))
            .ReturnsAsync(favoriteModels);
    
        //act
        var fetchedFavorites = await _favoriteDao.GetFavorites(accountId);
    
        //assert
        fetchedFavorites.Should().BeEquivalentTo(expectedFavoriteModels);
    }
}
