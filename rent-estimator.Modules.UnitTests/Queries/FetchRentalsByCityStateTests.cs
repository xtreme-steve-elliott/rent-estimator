using System.Threading.Tasks;
using FluentAssertions;
using rent_estimator.Modules.RentEstimation.Queries;
using Xunit;

namespace rent_estimator.Modules.UnitTests.Queries;

public class FetchRentalsByCityStateTests
{
    private readonly FetchRentalsByCityStateValidator _validator;

    public FetchRentalsByCityStateTests()
    {
        _validator = new FetchRentalsByCityStateValidator();
    }
    
    [Theory]
    [InlineData("validCity", "validStateAbbrev", true, null)]
    [InlineData("", "validStateAbbrev", false, "City must not be empty.")]
    [InlineData("validCity", "", false, "State abbreviation must not be empty.")]
    public async Task CreateAccountRequestValidator_ValidatesRequestPossibilities(
        string city,
        string stateAbbrev,
        bool isValid,
        string errorMessage
    )
    {
        //Arrange
        var request = new FetchRentalsByCityStateRequest
        {
            city = city,
            stateAbbrev = stateAbbrev
        };
        
        //Act
        var result = await _validator.ValidateAsync(request);
        
        //Assert
        result.IsValid.Should().Be(isValid);
        if(!isValid) result.Errors[0].ErrorMessage.Should().Be(errorMessage);
    }
}