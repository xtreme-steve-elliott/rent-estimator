using Dapper;
using Moq.Dapper;
using rent_estimator.Shared.Dapper;

namespace rent_estimator.Shared.UnitTests.Dapper;

using System.Data;

public class DapperWrapperTests
{
    private readonly string sqlQuery = "select Name from dbo.Groceries";

    [Fact]
    public void Wrapper_Should_BeAssignableToIDapperWrapper()
    {
        typeof(DapperWrapper).Should().BeAssignableTo<IDapperWrapper>();
    }

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

        dbConnection.SetupDapperAsync(dbConn =>
                dbConn.QueryAsync<string>(sqlQuery, null, null, null, It.IsAny<CommandType>()))
            .ReturnsAsync(expected);

        // Act
        var result = await sut.QueryAsync<string>(sqlQuery);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}