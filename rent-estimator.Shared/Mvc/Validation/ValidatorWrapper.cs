using FluentValidation;
using FluentValidation.Results;

namespace rent_estimator.Shared.Mvc.Validation;

public class ValidatorWrapper<TRequest> : IValidatorWrapper<TRequest>
{
    private readonly AbstractValidator<TRequest> _validator;
    
    public ValidatorWrapper(AbstractValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<ValidationResult> Validate(TRequest request, CancellationToken token)
    {
        return await _validator.ValidateAsync(request, token);
    }
}

public interface IValidatorWrapper<in TRequest>
{ 
    Task<ValidationResult> Validate(TRequest request, CancellationToken token);
}