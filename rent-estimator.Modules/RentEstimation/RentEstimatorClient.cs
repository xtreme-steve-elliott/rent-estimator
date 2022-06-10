namespace rent_estimator.Modules.RentEstimation;

public class RentEstimatorClient : IRentEstimatorClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiKey;
    
    public RentEstimatorClient(IHttpClientFactory clientFactory, string apiKey)
    {
        _clientFactory = clientFactory;
        _apiKey = apiKey;
    }

    public async Task<string> FetchRentalListingsByCityState(string city, string stateAbbrev)
    {
        var client = _clientFactory.CreateClient(nameof(RentEstimatorClient));
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "realty-in-us.p.rapidapi.com");
        var response = await client.GetAsync($"properties/v2/list-for-rent?city={city}&state_code={stateAbbrev}&limit=200&offset=0&sort=relevance");
        return await response.Content.ReadAsStringAsync();
    }
}

public interface IRentEstimatorClient
{
    Task<string> FetchRentalListingsByCityState(string city, string stateAbbrev);
}