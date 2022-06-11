namespace rent_estimator.Modules.RentEstimation;

public class RentEstimatorClient : IRentEstimatorClient
{
    private readonly IHttpClientFactory _clientFactory;
    
    public RentEstimatorClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<string> FetchRentalsByCityState(string city, string stateAbbrev)
    {
        var client = _clientFactory.CreateClient(nameof(RentEstimatorClient));
        var response = await client.GetAsync($"properties/v2/list-for-rent?city={city}&state_code={stateAbbrev}&limit=200&offset=0&sort=relevance");
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> FetchRental(string propertyId)
    {
        var client = _clientFactory.CreateClient(nameof(RentEstimatorClient));
        var response = await client.GetAsync($"properties/v2/detail?property_id={propertyId}");
        return await response.Content.ReadAsStringAsync();
    }
}

public interface IRentEstimatorClient
{
    Task<string> FetchRentalsByCityState(string city, string stateAbbrev);
    Task<string> FetchRental(string propertyId);
}