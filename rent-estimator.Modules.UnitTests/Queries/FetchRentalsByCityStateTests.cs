using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.RentEstimation;
using rent_estimator.Modules.RentEstimation.Queries;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Queries;

public class FetchRentalsByCityStateTests
{
    private readonly FetchRentalsByCityStateHandler _handler;
    private readonly Mock<IRentEstimatorClient> _client;
    private readonly FetchRentalsByCityStateValidator _validator;

    public FetchRentalsByCityStateTests()
    {
        _client = new Mock<IRentEstimatorClient>();
        _handler = new FetchRentalsByCityStateHandler(_client.Object);
        _validator = new FetchRentalsByCityStateValidator();
    }

    [Fact]
    public async void Handle_Should_InvokeRentEstimationClientReturnFetchRentalsByCityStateResponse()
    {
        //arrange
        const string city = "Chicago";
        const string stateAbbrev = "IL";
        var request = new FetchRentalsByCityStateRequest
        {
            city = city,
            stateAbbrev = stateAbbrev
        };
        const string content = "{ propertyId: testPropertyId }";
        var expected = new FetchRentalsByCityStateResponse
        {
            content = content
        };
        _client.Setup(client => client.FetchRentalsByCityState(city, stateAbbrev)).ReturnsAsync(content);
        
        //act
        var response = await _handler.Handle(request, new CancellationToken());
        
        //assert
        response.Should().BeEquivalentTo(expected);
        _client.Verify(client => client.FetchRentalsByCityState(city, stateAbbrev), Times.Once);
    }
    
    [Theory]
    [InlineData("validCity", "validStateAbbrev", true, null)]
    [InlineData("", "validStateAbbrev", false, "City must not be empty.")]
    [InlineData("validCity", "", false, "State abbreviation must not be empty.")]
    public async Task CreateAccountRequestValidator_ValidatesRequestPossibilities(
        string city,
        string stateAbbrev,
        bool isValid,
        string errorMessage
    )
    {
        //Arrange
        var request = new FetchRentalsByCityStateRequest
        {
            city = city,
            stateAbbrev = stateAbbrev
        };
        
        //Act
        var result = await _validator.ValidateAsync(request);
        
        //Assert
        result.IsValid.Should().Be(isValid);
        if(!isValid) result.Errors[0].ErrorMessage.Should().Be(errorMessage);
    }
}