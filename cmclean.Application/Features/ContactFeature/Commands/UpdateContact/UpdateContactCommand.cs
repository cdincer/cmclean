using MediatR;

namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact;

public class UpdateContactCommand : IRequest<UpdateContactResponse>
{
    public Guid Id { get; private set; }
    public string Salutation { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string DisplayName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Email { get; private set; }
    public string Phonenumber { get; private set; }

    public UpdateContactCommand(Guid id, string salutation, string firstName, string lastName, string displayName, DateTime birthDate, string email, string phonenumber)
    {
        Id = id;
        Salutation = salutation;
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        BirthDate = birthDate;
        Email = email;
        Phonenumber = phonenumber;
    }
}