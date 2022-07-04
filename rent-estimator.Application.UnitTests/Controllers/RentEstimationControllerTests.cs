using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.RentEstimation.Queries;
using Xunit;

namespace rent_estimator.Application.UnitTests.Controllers;

public class RentEstimationControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly IRentEstimationController _controller;

    public RentEstimationControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new RentEstimationController(_mediator.Object);
    }

    [Fact]
    public async void FetchRentalsByCityState_WithValidRequest_InvokesClientAndRespondsWith200AndBody()
    {
        //arrange
        const string city = "Chicago";
        const string stateAbbrev = "IL";
        const string content = "testContentAsString";
        var expected = new FetchRentalsByCityStateResponse {Content = content};
        _mediator.Setup(c => c.Send(It.IsAny<FetchRentalsByCityStateRequest>(), default)).ReturnsAsync(expected);

        //act
        var response = await _controller.FetchRentalsByCityState(city, stateAbbrev, default);

        //assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeEquivalentTo(expected);
        
        _mediator.Verify(m => m.Send(It.IsAny<FetchRentalsByCityStateRequest>(), default), Times.Once);
    }
    
    [Fact]
    public async void FetchRentalsByCityState_WithInvalidRequest_RespondsWith400AndErrorMessages()
    {
        const string city = "Chicago";
        const string stateAbbrev = "";
        var request = new FetchRentalsByCityStateRequest
        {
            City = city,
            StateAbbreviation = stateAbbrev
        };

        //act
        var response = await _controller.FetchRentalsByCityState(city, stateAbbrev, default);

        //assert
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        var result = response.Result as BadRequestObjectResult;
        result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        
        _mediator.Verify(m => m.Send(request, default), Times.Never);
    }

    [Fact]
    public async void GetRentalDetails_WithValidRequest_InvokesClientAndRespondsWith200AndBody()
    {
        //arrange
        const string propertyId = "M7952539079";
        var expected = new GetRentalDetailResponse
        {
            Content = "{content: asString}"
        };
        _mediator.Setup(m => m.Send(It.IsAny<GetRentalDetailRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);
        
        //act
        var response = await _controller.GetRentalDetail(propertyId, new CancellationToken());

        //assert
        _mediator.Verify(m => m.Send(It.IsAny<GetRentalDetailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeSameAs(expected);
    }
}