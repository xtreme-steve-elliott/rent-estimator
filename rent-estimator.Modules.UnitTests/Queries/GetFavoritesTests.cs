using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Modules.Favorite.Queries;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Queries;

public class GetFavoritesTests
{
    private readonly GetFavoritesHandler _handler;
    private readonly Mock<IFavoriteDao> _dao;

    public GetFavoritesTests()
    {
        _dao = new Mock<IFavoriteDao>();
        _handler = new GetFavoritesHandler(_dao.Object);
    }

    [Fact]
    public async void GetFavorites_Handle_ShouldInvokeFavoriteDaoAndReturnGetFavoritesResponse()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        const string propertyId = "M7952539079";
        var request = new GetFavoritesRequest { accountId = accountId };
        var favorite = new Favorite.Queries.Favorite
        {
            id = id,
            propertyId = propertyId,
            accountId = accountId
        };
        var favorites = new List<Favorite.Queries.Favorite> { favorite };
        var expected = new GetFavoritesResponse
        {
            favorites = favorites,
            Status = "Success"
        };
        var favoriteModel = new FavoriteModel
        {
            id = id,
            propertyId = propertyId,
            accountId = accountId
        };
        var model = new List<FavoriteModel>{  favoriteModel };
        _dao.Setup(dao => dao.GetFavorites(accountId)).ReturnsAsync(model);

        //act
        var response = await _handler.Handle(request, default);

        //assert
        response.Should().BeEquivalentTo(expected);
        _dao.Verify(dao => dao.GetFavorites(accountId), Times.Once);
    }
}