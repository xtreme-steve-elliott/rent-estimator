using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using rent_estimator.Controllers;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Shared.Mvc;
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
        var expected = new CreateAccountResponse();
        _mediator.Setup(m =>
            m.Send<CreateAccountResponse>(
                It.IsAny<CreateAccountRequest>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expected);

        //act
        var response = await _accountController.CreateAsync(
            new CreateAccountRequest(),
            new CancellationToken()
        );

        //Assert
        response.Result.Should().BeAssignableTo<OkObjectResult>();
        var result = response.Result as OkObjectResult;

        result?.Value.Should().BeSameAs(expected);
    }
    
    [Fact]
    public void GetByZipCodeAsync_ShouldBeDecoratedWith()
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
}