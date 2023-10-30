using MediatR;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactCommand : IRequest<CreateContactResponse>
{
    public string FirstName { get; private set; } 
    public string LastName { get; private  set; } 
    public DateTime DateOfBirth { get; private  set; }

    public CreateContactCommand(string firstName, string lastName,DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
}