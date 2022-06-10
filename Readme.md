# rent-estimator

## Technologies used

- .Net Core / C#
- MediatR
- Dapper
- Microsoft Sql Server
- Fluent Assertions
- Fluent Validations
- Moq
- Xunit
- WireMock.Net


## Getting started
 - Run `dotnet restore` from solution dir to download dependencies

 - Run `dotnet user-secrets set "ServiceClients:RentEstimatorApi:Key" "{{apiKeyValue}}" --project rent-estimator.Application/rent-estimator.Application.csproj"` to set the api key (local development only) for the Realty API that this project depends on

 - Run `cd Docker/msSql` and then `docker compose up -d` to build image & start mssql db container
 

 - Run `cd rent-estimator.Application` and then `dotnet run` to start application and open swagger-ui