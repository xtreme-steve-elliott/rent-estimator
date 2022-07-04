using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Modules.Account.Dao;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Commands;

public class CreateAccountTests
{
    public class CommandHandlerTests
    {
        private readonly Mock<IAccountDao> _accountDaoMock;
        private readonly CreateAccountCommandHandler _accountCommandHandler;

        public CommandHandlerTests()
        {
            _accountDaoMock = new Mock<IAccountDao>();
            _accountCommandHandler = new CreateAccountCommandHandler(_accountDaoMock.Object);
        }
        
        [Fact]
        public async Task Handle_ShouldCall_AccountDaoCreateAccount_AndReturn_CreateAccountResponse()
        {
            const string firstName = "firstName";
            const string lastName = "lastName";
            const string username = "tester1";
            const string password = "password123";
            //Arrange
            var request = new CreateAccountRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Password = password
            };
            var accountModel = new AccountModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password
                // Id will be generated
            };
            var expectedAccountModel = new AccountModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password
                // Id will be generated
            };
            AccountModel capturedAccountModel = new();
            var expectedResponse = new CreateAccountResponse
            {
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                Username = accountModel.Username,
                // Id will be generated
            };
            
            _accountDaoMock
                .Setup(_ => _.CreateAccount(It.IsAny<AccountModel>()))
                .Callback<AccountModel>(_ =>
                {
                    accountModel.Id = expectedAccountModel.Id = expectedResponse.Id = _.Id;
                    capturedAccountModel = _;
                })
                .ReturnsAsync(accountModel);

            //Act
            var response = await _accountCommandHandler.Handle(request, default);

            //Assert
            response.Should().BeEquivalentTo(expectedResponse);
            capturedAccountModel.Should().BeEquivalentTo(expectedAccountModel);
            _accountDaoMock.Verify(_ => _.CreateAccount(It.IsAny<AccountModel>()), Times.Once);
        }
    }

    public class RequestValidatorTests
    {
        private readonly CreateAccountRequestValidator _validator;

        public RequestValidatorTests()
        {
            _validator = new CreateAccountRequestValidator();
        }
        
        [Theory]
        [InlineData("Valid_Username-1", "Valid_Password-1", true, null)]
        [InlineData("InvalidUsername-1$#", "Valid_Password-1", false, "Username is not valid. Only ['A', 'a', '1', '-', '_'] are allowed.")]
        [InlineData("Valid_Username-1", "Invalid_Password-1#$", false, "Password is not valid. Only ['A', 'a', '1', '-', '_'] are allowed.")]
        [InlineData("", "Valid_Password-1", false, "Username must not be empty.")]
        [InlineData("Valid_Username-1", "", false, "Password must not be empty.")]
        public async Task ValidateAsync_ShouldReturn_ValidityAndAnyRelatedErrors(
            string username,
            string password,
            bool isValid,
            string errorMessage
        )
        {
            //Arrange
            var request = new CreateAccountRequest
            {
                Username = username,
                Password = password
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
