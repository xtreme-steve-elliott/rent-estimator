using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using rent_estimator.Modules.Account.Dao;
using rent_estimator.Shared.Dapper;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Dao;

public class AccountDaoTests
{
    private const string CreateSqlStatement = "create test";
    private readonly Mock<IDapperWrapper> _dbMock;
    private readonly Mock<IAccountSql> _accountSqlMock;
    private readonly IAccountDao _accountDao;

    public AccountDaoTests()
    {
        _dbMock = new Mock<IDapperWrapper>();
        _accountSqlMock = new Mock<IAccountSql>();
        _accountSqlMock
            .Setup(_ => _.CreateAccountSql())
            .Returns(CreateSqlStatement);
        _accountDao = new AccountDao(_dbMock.Object, _accountSqlMock.Object);
    }

    [Fact]
    public async Task CreateAccount_ShouldCall_AccountSqlCreateAccountSql_AndDbQueryFirstAsync()
    {
        //arrange
        var accountModel = new AccountModel
        {
            Id = Guid.NewGuid().ToString(),
            Username = "TestUsername",
            Password = "TestPassword",
            FirstName = "John",
            LastName = "Testerson"
        };
        
        //act
        await _accountDao.CreateAccount(accountModel);

        //assert
        _accountSqlMock.Verify(_ => _.CreateAccountSql(), Times.Once);
        _dbMock.Verify(_ => _.QueryFirstAsync<AccountModel>(CreateSqlStatement, accountModel), Times.Once);
    }
    
    [Fact]
    public async Task CreateAccount_ShouldReturn_CreatedAccount()
    {
        //arrange
        var accountModel = new AccountModel
        {
            Id = Guid.NewGuid().ToString(),
            Username = "TestUsername",
            Password = "TestPassword",
            FirstName = "John",
            LastName = "Testerson",
        };

        _dbMock
            .Setup(_ => _.QueryFirstAsync<AccountModel>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(accountModel);
        
        //act
        var createdAccount = await _accountDao.CreateAccount(accountModel);

        //assert
        createdAccount.Should().BeEquivalentTo(accountModel);
    }
}
