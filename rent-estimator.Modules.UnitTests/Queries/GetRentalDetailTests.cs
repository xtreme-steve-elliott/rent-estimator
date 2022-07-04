using System.Threading;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.RentEstimation;
using rent_estimator.Modules.RentEstimation.Queries;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Queries;

public class GetRentalDetailTests
{
    public class HandlerTests
    {
        private readonly Mock<IRentEstimatorClient> _clientMock;
        private readonly GetRentalDetailHandler _handler;
    
        public HandlerTests()
        {
            _clientMock = new Mock<IRentEstimatorClient>();
            _handler = new GetRentalDetailHandler(_clientMock.Object);
        }
        
        [Fact]
        public async void Handle_ShouldCall_RentEstimatorClient_AndReturn_GetRentalDetailResponse()
        {
            //arrange
            const string propertyId = "testPropertyId";
            const string content = "{testContent: asString}";
            var request = new GetRentalDetailRequest { PropertyId = propertyId };
            var expected = new GetRentalDetailResponse { Content = content };
            _clientMock.Setup(_ => _.FetchRental(propertyId)).ReturnsAsync(content);

            //act
            var response = await _handler.Handle(request, new CancellationToken());

            //assert
            response.Should().BeEquivalentTo(expected);
            _clientMock.Verify(_ => _.FetchRental(propertyId), Times.Once);
        }
    }
}
