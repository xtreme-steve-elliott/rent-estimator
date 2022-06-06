using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Shared.Mvc;
using rent_estimator.Shared.Mvc.Validation;
using Xunit;

namespace rent_estimator.Application.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IValidatorWrapper<CreateAccountRequest>> _validator;
    private readonly AccountController _accountController;

    public AccountControllerTests()
    {
        _validator = new Mock<IValidatorWrapper<CreateAccountRequest>>();
        _mediator = new Mock<IMediator>();
        _accountController = new AccountController(_mediator.Object, _validator.Object);
    }

    [Fact]
    public void AccountController_ShouldBeOfTypeApiControllerBaseAndIAccountController()
    {
        _accountController.Should().BeAssignableTo<IAccountController>();
        _accountController.Should().BeAssignableTo<ApiControllerBase>();
    }

    [Fact]
    public async Task CreateAsync_ShouldInvokeMediator_AndReturnCreateAccountResponse()
    {
        
        //arrange
        var expected = new CreateAccountResponse
        {
            Username = "Tester1",
            Id = "success-id",
            Status = "Success"
        };
        _mediator.Setup(m =>
            m.Send<CreateAccountResponse>(
                It.IsAny<CreateAccountRequest>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expected);

        _validator.Setup(v => v.Validate(It.IsAny<CreateAccountRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        //act
        var response = await _accountController.CreateAsync(
            new CreateAccountRequest(),
            new CancellationToken()
        );

        //Assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeSameAs(expected);
        
        _mediator.Verify(m => m.Send(
                It.IsAny<CreateAccountRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
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

    [Fact]
    public void CreateAsync_shouldInvokeValidator()
    {
        // Arrange
        var createAccountRequest = new CreateAccountRequest
        {
            Username = "PasswordCantBeEmpty",
            Password = ""
        };
        
        //Act
        _accountController.CreateAsync(createAccountRequest, default);

        //Assert
        _validator.Verify(v => v.Validate(createAccountRequest, default), Times.Once);
    }
}