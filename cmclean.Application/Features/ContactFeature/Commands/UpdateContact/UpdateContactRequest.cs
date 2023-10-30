namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact;

public class UpdateContactRequest
{
    public Guid ContactId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    
    public UpdateContactRequest(Guid contactId, string firstName, string lastName, DateTime dateOfBirth)
    {
        ContactId = contactId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
}