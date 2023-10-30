using MediatR;

namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact;

public class UpdateContactCommand : IRequest<UpdateContactResponse>
{
    public Guid ContactId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    
    public UpdateContactCommand(Guid contactId, string? firstName, string? lastName, DateTime dateOfBirth)
    {
        ContactId = contactId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
}