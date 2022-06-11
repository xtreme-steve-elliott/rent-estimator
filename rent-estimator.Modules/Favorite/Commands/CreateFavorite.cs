using FluentValidation;
using MediatR;
using rent_estimator.Modules.Favorite.Dao;
using rent_estimator.Shared.Documentation;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Modules.Favorite.Commands;

public class CreateFavoriteRequest: IRequest<CreateFavoriteResponse>
{
    [SwaggerSchema(Description = "AccountID for the user favoriting an estimate", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("e1f30249-bbba-4544-9f26-605c050294d8")]
    public string accountId { get; set; }
    
    [SwaggerSchema(Description = "PropertyId for the favorite being created", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("M7952539079")]
    public string propertyId { get; set; }
}

public class CreateFavoriteResponse: StandardResponse
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

public class CreateFavoriteCommandHandler: IRequestHandler<CreateFavoriteRequest, CreateFavoriteResponse>
{
    private readonly IFavoriteDao _dao;

    public CreateFavoriteCommandHandler(IFavoriteDao dao)
    {
        _dao = dao;
    }
        
    public async Task<CreateFavoriteResponse> Handle(CreateFavoriteRequest request, CancellationToken cancellationToken)
    {
        var modelToSave = new FavoriteModel
        {
            id = Guid.NewGuid().ToString(),
            accountId = request.accountId,
            propertyId = request.propertyId
        };
        var savedFavorite = await _dao.CreateFavorite(modelToSave);
        return new CreateFavoriteResponse
        {
            id = savedFavorite.id,
            accountId = savedFavorite.accountId,
            propertyId = savedFavorite.propertyId
        };
    }
}

public class CreateFavoriteRequestValidator : AbstractValidator<CreateFavoriteRequest>
{
    public CreateFavoriteRequestValidator()
    {
        RuleFor(p => p.accountId)
            .NotEmpty()
            .WithMessage("AccountId must not be empty.");

        RuleFor(p => p.propertyId)
            .NotEmpty()
            .WithMessage("PropertyId must not be empty.");
    }
}
