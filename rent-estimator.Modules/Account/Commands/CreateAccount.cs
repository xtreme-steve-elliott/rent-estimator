namespace rent_estimator.Modules.Account.Commands;

public class CreateAccountRequest: IRequest<CreateAccountResponse>
{
    [SwaggerSchema(Description = "First name of new user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("John")]
    public string? FirstName { get; set; }
    
    [SwaggerSchema(Description = "Last name of new user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("Anderson")]
    public string? LastName { get; set; }
    
    [SwaggerSchema(Description = "Username for the new user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("testuser123")]
    [Required]
    public string Username { get; set; }
    
    [SwaggerSchema(Description = "Password for the new user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("_Secret-Password.123")]
    [Required]
    public string Password { get; set; }
}

public class CreateAccountResponse: StandardResponse
{
    [SwaggerSchema(Description = "Identifier for the created user account", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("e1f30249-bbba-4544-9f26-605c050294d8")]
    public Guid Id { get; set; }
    
    [SwaggerSchema(Description = "First name of created user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("John")]
    public string? FirstName { get; set; }
    
    [SwaggerSchema(Description = "Last name of created user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("Anderson")]
    public string? LastName { get; set; }
    
    [SwaggerSchema(Description = "Username for the created user account", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("testuser123")]
    public string Username { get; set; }
}

public class CreateAccountCommandHandler: IRequestHandler<CreateAccountRequest, CreateAccountResponse>
{
    private readonly IAccountDao _accountDao;
    public CreateAccountCommandHandler(IAccountDao accountDao)
    {
        _accountDao = accountDao;
    }
    
    public async Task<CreateAccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var accountToSave = new AccountModel
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now
        };

        var savedAccount = await _accountDao.CreateAccount(accountToSave);
        return new CreateAccountResponse
        {
            Id = savedAccount.Id,
            Username = savedAccount.Username,
            FirstName = savedAccount.FirstName,
            LastName = savedAccount.LastName
        };
    }
}

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(p => p.Password)
            .Matches("^[a-zA-Z0-9_-]*$")
            .WithMessage("{PropertyName} is not valid. Only ['A', 'a', '1', '-', '_'] are allowed.");
        
        RuleFor(p => p.Username)
            .Matches("^[a-zA-Z0-9_-]*$")
            .WithMessage("{PropertyName} is not valid. Only ['A', 'a', '1', '-', '_'] are allowed.");
    }
}