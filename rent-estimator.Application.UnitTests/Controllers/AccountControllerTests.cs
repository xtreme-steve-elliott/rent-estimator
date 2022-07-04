using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.Account.Commands;
using Xunit;

namespace rent_estimator.Application.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly AccountController _accountController;

    public AccountControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _accountController = new AccountController(_mediator.Object);
    }

    // TODO: I don't think we need to test that we have a method that comes from the interface
    [Fact]
    public void CreateAccount_ShouldBeDecoratedWith()
    {
        var constraint = typeof(AccountController).Should()
            .HaveMethod(nameof(AccountController.CreateAsync),
                new[]
                {
                    typeof(CreateAccountRequest),
                    typeof(CancellationToken)
                });

        constraint.Which.Should().BeDecoratedWith<HttpPostAttribute>(r => r.Name == "CreateAccount", "required HttpPost with CreateAsync route");
    }
    
    #region CreateAsync
    
    [Fact]
    public async Task CreateAsync_WhenRequestIsValid_ShouldInvokeMediator_AndReturnCreateAccountResponse()
    {
        //arrange
        var request = new CreateAccountRequest
        {
            Username = "validUsername",
            Password = "validPassword"
        };
        var expected = new CreateAccountResponse
        {
            Username = "Tester1",
            Id = "success-id",
            Status = "Success"
        };
        _mediator.Setup(m =>
            m.Send(
                It.IsAny<CreateAccountRequest>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expected);

        //act
        var response = await _accountController.CreateAsync(
            request,
            new CancellationToken()
        );

        //Assert
        response.Result.Should().NotBeNull().And.BeAssignableTo<OkObjectResult>();
        var result = response.Result.As<OkObjectResult>();

        result?.Value.Should().BeSameAs(expected);
        
        _mediator.Verify(m => m.Send(
                It.IsAny<CreateAccountRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async void CreateAsync_WhenInvalidRequest_Returns400WithErrorMessage()
    {
        //arrange
        var invalidRequest = new CreateAccountRequest
        {
            Username = "PasswordIsInvalid",
            Password = ""
        };

        //act
        var response = await _accountController.CreateAsync(
            invalidRequest,
            new CancellationToken()
        );

        //Assert
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        var result = response.Result as BadRequestObjectResult;
        result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        _mediator.Verify(m => m.Send(
                It.IsAny<CreateAccountRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    #endregion
}