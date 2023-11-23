using FluentValidation;

namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact
{
    public class UpdateContactValidator : AbstractValidator<UpdateContactCommand>
    {
        public UpdateContactValidator()
        {
            RuleFor(_ => _.Salutation)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2);
            RuleFor(_ => _.FirstName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2);
            RuleFor(_ => _.LastName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2);
            RuleFor(_ => _.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
        }
    }
}
