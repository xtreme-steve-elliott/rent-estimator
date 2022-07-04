using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.Favorite.Commands;
using rent_estimator.Modules.Favorite.Queries;
using Xunit;

namespace rent_estimator.Application.UnitTests.Controllers;

public class FavoriteControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly IFavoriteController _controller;

    public FavoriteControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new FavoriteController(_mediator.Object);
    }

    // TODO: Probably don't need this
    [Fact]
    public void CreateFavorite_ShouldBeDecoratedWith()
    {
        var constraint = typeof(FavoriteController).Should()
            .HaveMethod(nameof(FavoriteController.CreateFavorite),
                new[]
                {
                    typeof(CreateFavoriteRequest),
                    typeof(CancellationToken)
                });

        constraint.Which.Should().BeDecoratedWith<HttpPostAttribute>(r => r.Name == "CreateFavorite", "required HttpPost with CreateAsync route");
    }

    [Fact]
    public async void CreateFavorite_WhenRequestIsValid_InvokesMediatRAndRespondsWithCreateFavoriteResponse()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        const string propertyId = "M7952539079";
        var validRequest = new CreateFavoriteRequest
        {
            AccountId = accountId,
            PropertyId = propertyId
        };
        var expected = new CreateFavoriteResponse
        {
            Id = favoriteId,
            AccountId = accountId,
            PropertyId = propertyId,
            Status = "Success"
        };
        _mediator.Setup(m =>
            m.Send(
                It.IsAny<CreateFavoriteRequest>(),
                It.IsAny<CancellationToken>()
        )).ReturnsAsync(expected);

        //act
        var response = await _controller.CreateFavorite(validRequest, new CancellationToken());

        //assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeSameAs(expected);
        
        _mediator.Verify(m => m.Send(
                It.IsAny<CreateFavoriteRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async void CreateFavorite_WhenRequestIsInvalid_RespondsWith400AndErrorMessage()
    {
        //arrange
        var invalidRequest = new CreateFavoriteRequest();

        //act
        var response = await _controller.CreateFavorite(
            invalidRequest,
            new CancellationToken()
        );

        //Assert
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        var result = response.Result as BadRequestObjectResult;
        result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        _mediator.Verify(m => m.Send(
                It.IsAny<CreateFavoriteRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async void GetFavorites_WhenRequestIsValid_InvokesMediatRAndRespondsWithGetFavoritesResponse()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        const string propertyId = "M7952539079";
        var favorite = new Favorite
        {
            Id = id,
            PropertyId = propertyId,
            AccountId = accountId
        };
        var favorites = new List<Favorite> { favorite };
        var expected = new GetFavoritesResponse
        {
            Favorites = favorites,
            Status = "Success"
        };
        _mediator.Setup(m =>
            m.Send(
                It.IsAny<GetFavoritesRequest>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expected);

        //act
        var response = await _controller.GetFavorites(accountId, new CancellationToken());

        //assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeSameAs(expected);
        
        _mediator.Verify(m => m.Send(
                It.IsAny<GetFavoritesRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}