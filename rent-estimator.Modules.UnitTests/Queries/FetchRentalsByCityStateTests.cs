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
    public class HandlerTests
    {
        private readonly Mock<IRentEstimatorClient> _clientMock;
        private readonly FetchRentalsByCityStateHandler _handler;

        public HandlerTests()
        {
            _clientMock = new Mock<IRentEstimatorClient>();
            _handler = new FetchRentalsByCityStateHandler(_clientMock.Object);
        }
        
        [Fact]
        public async void Handle_ShouldCall_RentEstimatorClientFetchRentalsByCityState_AndReturn_FetchRentalsByCityStateResponse()
        {
            //arrange
            const string city = "Chicago";
            const string stateAbbrev = "IL";
            var request = new FetchRentalsByCityStateRequest
            {
                City = city,
                StateAbbreviation = stateAbbrev
            };
            const string content = "{ propertyId: testPropertyId }";
            var expected = new FetchRentalsByCityStateResponse
            {
                Content = content
            };
            _clientMock
                .Setup(_ => _.FetchRentalsByCityState(city, stateAbbrev))
                .ReturnsAsync(content);
        
            //act
            var response = await _handler.Handle(request, new CancellationToken());
        
            //assert
            response.Should().BeEquivalentTo(expected);
            _clientMock.Verify(_ => _.FetchRentalsByCityState(city, stateAbbrev), Times.Once);
        }
    }

    public class ValidatorTests
    {
        private readonly FetchRentalsByCityStateValidator _validator;

        public ValidatorTests()
        {
            _validator = new FetchRentalsByCityStateValidator();
        }
        
        [Theory]
        [InlineData("validCity", "validStateAbbrev", true, null)]
        [InlineData("", "validStateAbbrev", false, "City must not be empty.")]
        [InlineData("validCity", "", false, "State Abbreviation must not be empty.")]
        public async Task ValidateAsync_ShouldReturn_ValidityAndAnyRelatedErrors(
            string city,
            string stateAbbrev,
            bool isValid,
            string errorMessage
        )
        {
            //Arrange
            var request = new FetchRentalsByCityStateRequest
            {
                City = city,
                StateAbbreviation = stateAbbrev
            };
        
            //Act
            var result = await _validator.ValidateAsync(request);
        
            //Assert
            result.IsValid.Should().Be(isValid);
            if (!isValid)
            {
                result.Errors.Should().NotBeEmpty();
                result.Errors[0].ErrorMessage.Should().Be(errorMessage);
            }
        }
    }
}
