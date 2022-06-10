using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.RentEstimation;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace rent_estimator.Modules.UnitTests;

public class RentEstimationClientTests
{
    private readonly IRentEstimatorClient _rentEstimatorClient;
    private readonly Mock<IHttpClientFactory> _clientFactory;
    
    public RentEstimationClientTests()
    {
        _clientFactory = new Mock<IHttpClientFactory>();
        _rentEstimatorClient = new RentEstimatorClient(_clientFactory.Object, "apiKey");
    }

    [Fact]
    public void RentEstimatorClient_ImplementsTheIRentEstimatorInterface()
    {
        _rentEstimatorClient.Should().BeAssignableTo<IRentEstimatorClient>();
    }
    
    [Fact(Skip = "Wire mock server cannot match with given information")]
    public async void FetchRentalsByCityState_WhenRequestIsValid_InvokesRentEstimatorClientAndReturnsListings()
    {
        //arrange
        using var rentEstimationServer = WireMockServer.Start(port: 2011);
        
        const string city = "Chicago";
        const string stateAbbrev = "IL";
        var expected = new { propertyId = "testPropertyId" }.ToString();
        var path = HttpUtility.UrlEncode($"/properties/v2/list-for-rent?city={city}&state_code={stateAbbrev}&limit=200&offset=0&sort=relevance");
        
        rentEstimationServer
            .Given(Request.Create()
                .WithPath(path)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBody(expected)
            );
        
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:2011");

        _clientFactory.Setup(f => f.CreateClient(nameof(RentEstimatorClient))).Returns(client);

        //act
        var response = await _rentEstimatorClient.FetchRentalListingsByCityState(city, stateAbbrev);
        
        //assert
        response.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void FetchRentalsByCityState_WhenRentEstimatorServiceThrowsException_ReturnFailureResponse()
    {
        
    }
}