namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactRequest
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    public CreateContactRequest(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
}