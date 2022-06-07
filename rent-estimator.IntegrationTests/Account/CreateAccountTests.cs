using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Shared.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace rent_estimator.IntegrationTests.Account;

[Collection(nameof(CreateAccountTests))]
public class CreateAccountTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly WebApplicationFactory<Program> _factory;

    public CreateAccountTests(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _factory = factory.WithWebHostBuilder(builder => { builder.WithDefaultBuilderSettings(_outputHelper); });
    }

    [Fact]
    public async void GivenNewAccount_WhenValidRequestReceived_ThenCreatedAccountSavedInDBAndReturned()
    {
        //arrange
        var client = _factory.CreateClient();
        var account = new CreateAccountRequest
        {
            Username = "testUsername",
            Password = "testPassword",
            FirstName = "Jon",
            LastName = "Smith"
        };
        
        HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

        //act
        var response = await client.PostAsync("/accounts", requestBody);
        
        //assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStringAsync();
        //outputHelper.WriteLine(responseBody);

        //var body = JsonSerializer.Deserialize<CreateAccountResponse>(responseBody, SerializationHelper.GetSerializerOptions());
        var bodyContent = JsonConvert.DeserializeObject<CreateAccountResponse>(responseBody);
        bodyContent.Id.Should().NotBeEmpty();
        bodyContent.Username.Should().Be(account.Username);
        bodyContent.FirstName.Should().Be(account.FirstName);
        bodyContent.LastName.Should().Be(account.LastName);

    }
    
    [Fact]
    public async void GivenNewAccount_WhenInvalidRequestReceived_ReturnsErrorResponse()
    {
        //arrange
        var client = _factory.CreateClient();
        var account = new CreateAccountRequest
        {
            Username = "testUsername",
            Password = "testPassword",
            FirstName = "Jon",
            LastName = "Smith"
        };
        
        HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

        //act
        var response = await client.PostAsync("/accounts", requestBody);
        
        //assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var bodyContent = await response.Content.ReadAsStringAsync();
        //outputHelper.WriteLine(bodyContent);

        var body = JsonConvert.DeserializeObject<StandardErrorResponse>(bodyContent);
        body.StatusDetails.Count.Should().BeGreaterThan(0);
        var error = body.StatusDetails[0];
        error.Should().NotBeNull();
        // error.FieldName.Should().Be();
        // error.Description.Should().Be();
        // error.ProblemDetails.Should().Be();
    } 
}