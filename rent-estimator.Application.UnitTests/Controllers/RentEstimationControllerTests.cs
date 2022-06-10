using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.RentEstimation;
using rent_estimator.Modules.RentEstimation.Queries;
using rent_estimator.Shared.Mvc;
using Xunit;

namespace rent_estimator.Application.UnitTests.Controllers;

public class RentEstimationControllerTests
{
    private readonly Mock<IRentEstimatorClient> _client;
    private readonly IRentEstimationController _controller;

    public RentEstimationControllerTests()
    {
        _client = new Mock<IRentEstimatorClient>();
        _controller = new RentEstimationController(_client.Object);
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
        _client.Setup(c => c.FetchRentalListingsByCityState(city, stateAbbrev)).ReturnsAsync(content);

        //act
        var response = await _controller.FetchRentalsByCityState(request);

        //assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeEquivalentTo(expected);
        
        _client.Verify(m => m.FetchRentalListingsByCityState(city, stateAbbrev), Times.Once);
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
        var response = await _controller.FetchRentalsByCityState(request);

        //assert
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        var result = response.Result as BadRequestObjectResult;
        result?.StatusCode.Should().Be(400);
        
        _client.Verify(m => m.FetchRentalListingsByCityState(city, stateAbbrev), Times.Never);
    }
}