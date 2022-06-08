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
    private readonly CreateFavoriteRequestValidator _validator;
    private readonly Mock<IFavoriteDao> _favoriteDao;
    private readonly CreateFavoriteCommandHandler _handler;
    
    public CreateFavoriteTests()
    {
        _validator = new CreateFavoriteRequestValidator();
        _favoriteDao = new Mock<IFavoriteDao>();
        _handler = new CreateFavoriteCommandHandler(_favoriteDao.Object);
    }
    
    [Fact]
    public async Task CreateFavoriteCommandHandler_Handle_ShouldInvokeFavoriteDaoAndReturnCreateFavoriteResponse()
    {
        //arrange
        var accountId = Guid.NewGuid().ToString();
        var favoriteId = Guid.NewGuid().ToString();
        var requestFavorite = new CreateFavoriteRequest
        {
            accountId = accountId
        };
        var expected = new CreateFavoriteResponse
        {
            id = favoriteId,
            accountId = accountId
        };
        var favoriteModel = new FavoriteModel
        {
            id = favoriteId,
            accountId = accountId
        };
        _favoriteDao.Setup(dao => dao.CreateFavorite(It.IsAny<FavoriteModel>())).ReturnsAsync(favoriteModel);

        //act
        var savedFavorite = await _handler.Handle(requestFavorite, new CancellationToken());

        //assert
        savedFavorite.Should().BeEquivalentTo(expected);
        _favoriteDao.Verify(dao => dao.CreateFavorite(It.IsAny<FavoriteModel>()), Times.Once);
    }
    
    [Theory]
    [InlineData("valid-account-id", true, null)]
    [InlineData("", false, "AccountId must not be empty.")]
    public async Task CreateAccountRequestValidator_ValidatesRequestPossibilities(
        string accountId,
        bool isValid,
        string errorMessage
    )
    {
        //Arrange
        var request = new CreateFavoriteRequest
        {
            accountId = accountId
        };
        
        //Act
        var result = await _validator.ValidateAsync(request);
        
        //Assert
        result.IsValid.Should().Be(isValid);
        if(!isValid) result.Errors[0].ErrorMessage.Should().Be(errorMessage);
    }
}