using System.Threading;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.RentEstimation;
using rent_estimator.Modules.RentEstimation.Queries;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Queries;

public class GetRentalDetailTests
{
    private readonly Mock<IRentEstimatorClient> _client;
    private readonly GetRentalDetailHandler _handler;
    
    public GetRentalDetailTests()
    {
        _client = new Mock<IRentEstimatorClient>();
        _handler = new GetRentalDetailHandler(_client.Object);
    }

    [Fact]
    public async void GetRentalDetailHandler_Handle_InvokesRentEstimationClientAndRespondsWithContent()
    {
        //arrange
        const string propertyId = "testPropertyId";
        const string content = "{testContent: asString}";
        var request = new GetRentalDetailRequest { propertyId = propertyId };
        var expected = new GetRentalDetailResponse {content = content};
        _client.Setup(c => c.FetchRental(propertyId)).ReturnsAsync(content);

        //act
        var response = await _handler.Handle(request, new CancellationToken());

        //assert
        response.Should().BeEquivalentTo(expected);
        _client.Verify(c => c.FetchRental(propertyId), Times.Once);
    } 
}