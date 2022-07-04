using System;
using System.Net.Http;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.RentEstimation;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace rent_estimator.Modules.UnitTests;

// TODO: Haven't really looked at this yet.
public class RentEstimationClientTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly IRentEstimatorClient _rentEstimatorClient;

    public RentEstimationClientTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _rentEstimatorClient = new RentEstimatorClient(_httpClientFactoryMock.Object);
    }

    [Fact]
    public async void FetchRentalsByCityState_WhenRequestIsValid_InvokesClientAndReturnsListings()
    {
        //arrange
        using var rentEstimationServer = WireMockServer.Start(port: 2011);
        
        const string city = "Chicago";
        const string stateAbbrev = "IL";
        var expected = new { propertyId = "testPropertyId" }.ToString();
        
        rentEstimationServer
            .Given(Request.Create()
                .WithPath("/properties/v2/list-for-rent")
                .WithParam("city", city)
                .WithParam("state_code", stateAbbrev)
                .WithParam("limit", "200")
                .WithParam("offset", "0")
                .WithParam("sort", "relevance")
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBody(expected)
            );
        
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:2011");

        _httpClientFactoryMock
            .Setup(_ => _.CreateClient(nameof(RentEstimatorClient)))
            .Returns(client);

        //act
        var response = await _rentEstimatorClient.FetchRentalsByCityState(city, stateAbbrev);
        
        //assert
        response.Should().BeEquivalentTo(expected);
    }
    
    [Fact(Skip = "Empty")]
    public void FetchRentalsByCityState_WhenRentEstimatorServiceThrowsException_ReturnFailureResponse()
    {
        
    }

    [Fact]
    public async void FetchRental_WhenRequestIsValid_InvokesRentEstimatorAndReturnsPropertyDetail()
    {
        //arrange
        using var rentEstimationServer = WireMockServer.Start(port: 2011);
        
        const string propertyId = "propertyIdValue";
        const string expected = "{ content: contentValue}";
        
        rentEstimationServer
            .Given(Request.Create()
                .WithPath("/properties/v2/detail")
                .WithParam("property_id", propertyId)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBody(expected)
            );
        
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:2011");

        _httpClientFactoryMock.Setup(f => f.CreateClient(nameof(RentEstimatorClient))).Returns(client);

        //act
        var response = await _rentEstimatorClient.FetchRental(propertyId);
        
        //assert
        response.Should().BeEquivalentTo(expected);
    }
    
    [Fact(Skip = "Empty")]
    public async void FetchRental_WhenRentEstimatorServiceThrowsException_ReturnFailureResponse()
    {
        
    }
}