using MediatR;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Shared.Documentation;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Modules.Favorite.Queries;

public class GetFavoritesRequest : IRequest<GetFavoritesResponse>
{
    [SwaggerSchema(Description = "AccountID for the user getting all their favorites", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("e1f30249-bbba-4544-9f26-605c050294d8")]
    public string accountId { get; set; }
}

public class GetFavoritesResponse : StandardResponse
{
    [SwaggerSchema(Description = "Rent estimation response content as string", Format = "{xxx: xxx}", ReadOnly = true)]
    [SwaggerSchemaExample("{ propertyDetail: {propertyDetailObject} }")]
    public IEnumerable<Favorite> favorites { get; set; }
}

public class Favorite
{
    [SwaggerSchema(Description = "Identifier for the created 'favorited' search", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("e1f30249-bbba-4544-9f26-605c050294d8")]
    public string id { get; set; }
    
    [SwaggerSchema(Description = "Identifier for the user's account", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("e1f30249-bbba-4544-9f26-605c050294d8")]
    public string accountId { get; set; }
    
    [SwaggerSchema(Description = "Identifier for the 'favorited' piece of real estate", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("M7952539079")]
    public string propertyId { get; set; }
}

public class GetFavoritesHandler : IRequestHandler<GetFavoritesRequest, GetFavoritesResponse>
{
    private readonly IFavoriteDao _dao;

    public GetFavoritesHandler(IFavoriteDao dao)
    {
        _dao = dao;
    }
    
    public async Task<GetFavoritesResponse> Handle(GetFavoritesRequest request, CancellationToken token)
    {
        var favoritesFromDb = await _dao.GetFavorites(request.accountId);
        var favorites = favoritesFromDb.Select(model => 
            new Favorite {id = model.id, accountId = model.accountId, propertyId = model.propertyId}).ToList();
        return new GetFavoritesResponse {favorites = favorites};
    }
}