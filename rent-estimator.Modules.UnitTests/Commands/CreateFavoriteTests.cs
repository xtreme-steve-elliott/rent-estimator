using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.Favorite.Commands;
using rent_estimator.Modules.Favorite.Dao;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Commands;

public class CreateFavoriteTests
{
    public class CommandHandlerTests
    {
        private readonly Mock<IFavoriteDao> _favoriteDao;
        private readonly CreateFavoriteCommandHandler _handler;
        
        public CommandHandlerTests()
        {
            _favoriteDao = new Mock<IFavoriteDao>();
            _handler = new CreateFavoriteCommandHandler(_favoriteDao.Object);
        }
        
        [Fact]
        public async Task Handle_ShouldCall_FavoriteDaoCreateFavorite_AndReturn_CreateFavoriteResponse()
        {
            //arrange
            var accountId = Guid.NewGuid().ToString();
            const string propertyId = "M7952539079";
            var request = new CreateFavoriteRequest
            {
                AccountId = accountId,
                PropertyId = propertyId
            };
            var favoriteModel = new FavoriteModel
            {
                // Id will be generated
                AccountId = request.AccountId,
                PropertyId = request.PropertyId
            };
            var expectedFavoriteModel = new FavoriteModel
            {
                // Id will be generated
                AccountId = request.AccountId,
                PropertyId = request.PropertyId
            };
            FavoriteModel capturedFavoriteModel = new();
            var expectedResponse = new CreateFavoriteResponse
            {
                // Id will be generated
                AccountId = favoriteModel.AccountId,
                PropertyId = favoriteModel.PropertyId
            };
            _favoriteDao
                .Setup(_ => _.CreateFavorite(It.IsAny<FavoriteModel>()))
                .Callback<FavoriteModel>(_ =>
                {
                    favoriteModel.Id = expectedFavoriteModel.Id = expectedResponse.Id = _.Id;
                    capturedFavoriteModel = _;
                })
                .ReturnsAsync(favoriteModel);

            //act
            var response = await _handler.Handle(request, new CancellationToken());

            //assert
            response.Should().BeEquivalentTo(expectedResponse);
            capturedFavoriteModel.Should().BeEquivalentTo(expectedFavoriteModel);
            _favoriteDao.Verify(_ => _.CreateFavorite(It.IsAny<FavoriteModel>()), Times.Once);
        }
    }

    public class RequestValidatorTests
    {
        private readonly CreateFavoriteRequestValidator _validator;
    
        public RequestValidatorTests()
        {
            _validator = new CreateFavoriteRequestValidator();
        }
        
        [Theory]
        [InlineData("valid-account-id", "M7952539079", true, null)]
        [InlineData("", "M7952539079", false, "Account Id must not be empty.")]
        [InlineData("valid-account-id", "", false, "Property Id must not be empty.")]
        public async Task ValidateAsync_ShouldReturn_ValidityAndAnyRelatedErrors(
            string accountId,
            string propertyId,
            bool isValid,
            string errorMessage
        )
        {
            //Arrange
            var request = new CreateFavoriteRequest
            {
                AccountId = accountId,
                PropertyId = propertyId
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
