using System.Data;
using System.Data.SqlClient;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using rent_estimator.Controllers;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Modules.Account.Dao;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Modules.RentEstimation;
using rent_estimator.Shared.Dapper;

var requiredEnvironmentVariables = new[] {"DB_PASSWORD"};
CheckEnvironmentVariablesExist(requiredEnvironmentVariables);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient(nameof(RentEstimatorClient),
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetSection("ServiceClients")["RentEstimatorApi:BaseUri"]);
    });
builder.Services.AddSingleton<IRentEstimatorClient>(client =>
    new RentEstimatorClient(
        client.GetRequiredService<IHttpClientFactory>(), 
        builder.Configuration.GetSection("ServiceClients")["RentEstimatorApi:Key"]
        )
);
builder.Services.AddSingleton<IRentEstimationController>(controller => new RentEstimationController(controller.GetRequiredService<IRentEstimatorClient>()));

builder.Services.AddTransient(c => GetSqlConnection(builder.Configuration));
builder.Services.AddSingleton<IDapperWrapper, DapperWrapper>();
builder.Services.AddSingleton<IAccountSql, AccountSql>();
builder.Services.AddSingleton<IAccountDao>(dao => 
    new AccountDao(
        dao.GetRequiredService<IDapperWrapper>(), 
        dao.GetRequiredService<IAccountSql>())
);
builder.Services.AddSingleton<IFavoriteSql, FavoriteSql>();
builder.Services.AddSingleton<IFavoriteDao>(dao => 
    new FavoriteDao(
        dao.GetRequiredService<IDapperWrapper>(), 
        dao.GetRequiredService<IFavoriteSql>())
);

builder.Services.AddTransient<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
    fv.DisableDataAnnotationsValidation = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(CreateAccountCommandHandler));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void CheckEnvironmentVariablesExist(IEnumerable<string> envVars)
{
    foreach (var envVar in envVars)
        GetEnvironmentVariableOrThrow(envVar);
}

static IDbConnection GetSqlConnection(IConfiguration configuration)
{
    var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("rentEstimatorDB"))
        {
            Password = GetEnvironmentVariableOrThrow("DB_PASSWORD")
        };

    return new SqlConnection(connectionStringBuilder.ConnectionString);
}

static string GetEnvironmentVariableOrThrow(string envVarName)
{
    var value = Environment.GetEnvironmentVariable(envVarName);

    if (value != null) return value;

    throw new Exception($"Please set the environment variable '{envVarName}' on your machine");
}