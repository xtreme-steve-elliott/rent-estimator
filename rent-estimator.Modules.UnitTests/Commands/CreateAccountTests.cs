namespace rent_estimator.Modules.UnitTests.Commands;

public class CreateAccountTests
{
    private readonly CreateAccountCommandHandler _accountCommandHandler;
    private readonly Mock<IAccountDao> _accountDao;
    private readonly CreateAccountRequestValidator _validator;
    
    public CreateAccountTests()
    {
        _accountDao = new Mock<IAccountDao>();
        _accountCommandHandler = new CreateAccountCommandHandler(_accountDao.Object);
        _validator = new CreateAccountRequestValidator();
    }

    [Fact]
    public async Task CreateAccountCommandHandler_Handle_ShouldInvokeAccountDAOAndReturnCreateAccountResponse()
    {
        var id = Guid.NewGuid();
        //Arrange
        var requestBody = new CreateAccountRequest
        {
            FirstName = "firstName",
            LastName = "lastName",
            Username = "tester1",
            Password = "password123"
        };
        var expectedResponse = new CreateAccountResponse
        {
            FirstName = "firstName",
            LastName = "lastName",
            Username = "tester1",
            Id = id
        };
        var model = new AccountModel
        {
            FirstName = "firstName",
            LastName = "lastName",
            Username = "tester1",
            Id = id
        };
        _accountDao.Setup(dao => dao.CreateAccount(It.IsAny<AccountModel>())).ReturnsAsync(model);

        //Act
        var response = await _accountCommandHandler.Handle(requestBody, default);

        //Assert
        response.Should().BeEquivalentTo(expectedResponse);
        _accountDao.Verify(dao => dao.CreateAccount(It.IsAny<AccountModel>()), Times.Once);
    }

    [Theory]
    [InlineData("Valid_Username-1", "Valid_Password-1", true, null)]
    [InlineData("InvalidUsername-1$#", "Valid_Password-1", false, "Username is not valid. Only ['A', 'a', '1', '-', '_'] are allowed.")]
    [InlineData("Valid_Username-1", "Invalid_Password-1#$", false, "Password is not valid. Only ['A', 'a', '1', '-', '_'] are allowed.")]
    public async Task CreateAccountRequestValidator_ValidatesRequestPossibilities(
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
        if(!isValid) result.Errors[0].ErrorMessage.Should().Be(errorMessage);
    }
}