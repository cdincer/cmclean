using FluentValidation;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactValidator: AbstractValidator<CreateContactCommand>
{
    public CreateContactValidator()
    {
        RuleFor(_ => _.FirstName)
            .NotNull()
            .NotEmpty();
        RuleFor(_ => _.LastName)
            .NotNull()
            .NotEmpty();
    }
}