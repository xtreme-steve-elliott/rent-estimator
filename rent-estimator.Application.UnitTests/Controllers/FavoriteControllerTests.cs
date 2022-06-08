using System;
using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.Favorite.Commands;
using rent_estimator.Shared.Mvc;
using Xunit;

namespace rent_estimator.Application.UnitTests.Controllers;

public class FavoriteControllerTests
{
    private readonly IFavoriteController _controller;
    private readonly Mock<IMediator> _mediator;

    public FavoriteControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new FavoriteController(_mediator.Object);
    }
    
    [Fact]
    public void CreateFavorite_ShouldBeOfTypeIFavoriteControllerAndApiControllerBase()
    {
        _controller.Should().BeAssignableTo<IFavoriteController>();
        _controller.Should().BeAssignableTo<ApiControllerBase>();
    }
    
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
        var validRequest = new CreateFavoriteRequest
        {
            accountId = accountId
        };
        var expected = new CreateFavoriteResponse
        {
            id = favoriteId,
            accountId = accountId,
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
        var accountId = Guid.NewGuid().ToString();
        var invalidRequest = new CreateFavoriteRequest();

        //act
        var response = await _controller.CreateFavorite(
            invalidRequest,
            new CancellationToken()
        );

        //Assert
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        var result = response.Result as BadRequestObjectResult;
        result?.StatusCode.Should().Be(400);

        _mediator.Verify(m => m.Send(
                It.IsAny<CreateFavoriteRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}