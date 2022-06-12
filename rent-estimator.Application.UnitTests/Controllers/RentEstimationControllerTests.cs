using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.RentEstimation.Queries;
using rent_estimator.Shared.Mvc;
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
    public void RentEstimationController_IsAssignableToBaseControllerAndIRentEstimationController()
    {
        _controller.Should().BeAssignableTo<IRentEstimationController>();
        _controller.Should().BeAssignableTo<ApiControllerBase>();
    }

    [Fact]
    public async void FetchRentalsByCityState_WithValidRequest_InvokesClientAndRespondsWith200AndBody()
    {
        //arrange
        const string city = "Chicago";
        const string stateAbbrev = "IL";
        var request = new FetchRentalsByCityStateRequest
        {
            city = city,
            stateAbbrev = stateAbbrev
        };
        const string content = "testContentAsString";
        var expected = new FetchRentalsByCityStateResponse {content = content};
        _mediator.Setup(c => c.Send(request, default)).ReturnsAsync(expected);

        //act
        var response = await _controller.FetchRentalsByCityState(request, default);

        //assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeEquivalentTo(expected);
        
        _mediator.Verify(m => m.Send(request, default), Times.Once);
    }
    
    [Fact]
    public async void FetchRentalsByCityState_WithInvalidRequest_RespondsWith400AndErrorMessages()
    {
        const string city = "Chicago";
        const string stateAbbrev = "";
        var request = new FetchRentalsByCityStateRequest
        {
            city = city,
            stateAbbrev = stateAbbrev
        };

        //act
        var response = await _controller.FetchRentalsByCityState(request, default);

        //assert
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        var result = response.Result as BadRequestObjectResult;
        result?.StatusCode.Should().Be(400);
        
        _mediator.Verify(m => m.Send(request, default), Times.Never);
    }

    [Fact]
    public async void GetRentalDetails_WithValidRequest_InvokesClientAndRespondsWith200AndBody()
    {
        //arrange
        const string propertyId = "M7952539079";
        var expected = new GetRentalDetailResponse
        {
            content = "{content: asString}"
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