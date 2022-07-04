using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Modules.Favorite.Queries;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Queries;

public class GetFavoritesTests
{
    private readonly Mock<IFavoriteDao> _daoMock;
    private readonly GetFavoritesHandler _handler;

    public GetFavoritesTests()
    {
        _daoMock = new Mock<IFavoriteDao>();
        _handler = new GetFavoritesHandler(_daoMock.Object);
    }

    [Fact]
    public async void Handle_ShouldCall_FavoriteDaoGetFavorites_AndReturnGetFavoritesResponse()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var request = new GetFavoritesRequest
        {
            AccountId = accountId
        };
        var foundFavoriteModels = new List<FavoriteModel>
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                PropertyId = "property1",
                AccountId = accountId
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                PropertyId = "property2",
                AccountId = accountId
            }
        };
        var foundFavorites = foundFavoriteModels.Select(_ => new Favorite.Queries.Favorite
        {
            Id = _.Id,
            PropertyId = _.PropertyId,
            AccountId = _.AccountId
        });
        var expectedResponse = new GetFavoritesResponse
        {
            Favorites = foundFavorites,
            // TODO: Don't like status like this
            Status = "Success"
        };

        _daoMock
            .Setup(_ => _.GetFavorites(It.IsAny<string>()))
            .ReturnsAsync(foundFavoriteModels);
        
        //act
        var response = await _handler.Handle(request, default);

        //assert
        response.Should().BeEquivalentTo(expectedResponse);
        _daoMock.Verify(_ => _.GetFavorites(accountId), Times.Once);
    }
}
