using FluentValidation;

namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact
{
    public class UpdateContactValidator: AbstractValidator<UpdateContactCommand>
    {
        public UpdateContactValidator()
        {
            RuleFor(_=> _.FirstName)
                .NotEmpty()
                .NotNull();

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .NotNull();
        }
    }
}
