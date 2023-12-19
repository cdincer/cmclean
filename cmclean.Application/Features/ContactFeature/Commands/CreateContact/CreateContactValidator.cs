using AutoMapper;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using FluentValidation;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactValidator : AbstractValidator<CreateContactCommand>
{
    private readonly IContactReadRepository _ContactReadRepository;

    public CreateContactValidator(IContactReadRepository ContactReadRepository)
    {  
        _ContactReadRepository = ContactReadRepository;

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

        RuleFor(x => x.Email).Custom(async (checkEmail, context) => {
            string temp = checkEmail;
           var uniqueCheckResults =  await _ContactReadRepository.GetAsync(new GetContactByFilterQuery()
            {
                FirstName = "",
                LastName = "",
                DisplayName = "",
                BirthDate = DateTime.MinValue,
                Email = temp,
                Phonenumber = ""
            });

            if(uniqueCheckResults != null && uniqueCheckResults.Count != 0 )
            context.AddFailure("Email must be unique");
        });

    }
}