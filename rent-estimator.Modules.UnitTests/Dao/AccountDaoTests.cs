using System;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using rent_estimator.Modules.Account.Dao;
using rent_estimator.Shared.Dapper;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Dao;

public class AccountDaoTests
{
    private readonly Mock<IDapperWrapper> _db;
    private readonly IAccountDao _accountDao;
    private readonly AccountSql _accountSql;

    public AccountDaoTests()
    {
        _accountSql = new AccountSql();
        _db = new Mock<IDapperWrapper>();
        _accountDao = new AccountDao(_db.Object, _accountSql);
    }

    [Fact]
    public void AccountDao_ImplementsIAccountDaoInterface()
    {
        _accountDao.Should().BeAssignableTo<IAccountDao>();
    }
    
    [Fact]
    public async void AccountDao_WhenQueryRuns_InvokesDbConnection()
    {
        //arrange
        var accountModel = new AccountModel
        {
            Id = Guid.NewGuid(),
            Username = "TestUsername",
            Password = "TestPassword",
            FirstName = "John",
            LastName = "Testerson",
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now
        };
        var param = new
        {
            Id = accountModel.Id,
            Username =accountModel.Username,
            Password =accountModel.Password,
            FirstName = accountModel.FirstName,
            LastName = accountModel.LastName,
            CreatedAt = accountModel.CreatedAt,
            LastUpdatedAt = accountModel.LastUpdatedAt
        };
        var sqlQuery = _accountSql.CreateAccountSql();
        
        //act
        await _accountDao.CreateAccount(accountModel);

        //assert
        _db.Verify(c => c.QueryFirstAsync<AccountModel>(
                sqlQuery, 
                It.Is<object>(p => JsonConvert.SerializeObject(param) == JsonConvert.SerializeObject(p)) 
            ), Times.Once);
    }
    
    [Fact]
    public async void AccountDao_CreateAccount_SavesAndReturnsCreatedAccount()
    {
        //arrange
        var accountModel = new AccountModel
        {
            Id = Guid.NewGuid(),
            Username = "TestUsername",
            Password = "TestPassword",
            FirstName = "John",
            LastName = "Testerson",
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now
        };
        var sqlQuery = _accountSql.CreateAccountSql();

        _db.Setup(c => c.QueryFirstAsync<AccountModel>(sqlQuery, It.IsAny<object>()))
            .ReturnsAsync(accountModel);
        
        //act
        var createdAccount = await _accountDao.CreateAccount(accountModel);

        //assert
        createdAccount.Should().BeEquivalentTo(accountModel);
    }
}