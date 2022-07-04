using System.Collections.Generic;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using rent_estimator.Shared.Dapper;
using Xunit;

namespace rent_estimator.Shared.UnitTests.Dapper;

using System.Data;

public class DapperWrapperTests
{
    private const string sqlQuery = "select Name from dbo.Groceries";

    [Fact]
    public async void QueryAsync_WhenGivenAValidSqlQuery_ItShouldExecuteTheQueryAsynchronously()
    {
        // Arrange
        var dbConnection = new Mock<IDbConnection>();

        var sut = new DapperWrapper(dbConnection.Object);

        var expected = new List<string>
        {
            "Bananas",
            "Apples"
        };

        var param = new { };

        dbConnection.SetupDapperAsync(dbConn =>
                dbConn.QueryAsync<string>(sqlQuery, param, null, null, It.IsAny<CommandType>()))
            .ReturnsAsync(expected);

        // Act
        var result = await sut.QueryAsync<string>(sqlQuery, param);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
