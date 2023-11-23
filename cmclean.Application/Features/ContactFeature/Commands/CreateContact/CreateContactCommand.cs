using MediatR;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactCommand : IRequest<CreateContactResponse>
{
    public string Salutation { get; private set; }
    public string FirstName { get; private set; } 
    public string LastName { get; private set; }
    public string DisplayName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Email { get; set; }
    public string Phonenumber { get; set; }

    public CreateContactCommand(string salutation,string firstName, string lastName, string displayName,
                               DateTime birthDate, string email, string phonenumber)
    {
        Salutation = salutation;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        DisplayName = displayName;
        Email = email;
        Phonenumber = phonenumber;
    }
}