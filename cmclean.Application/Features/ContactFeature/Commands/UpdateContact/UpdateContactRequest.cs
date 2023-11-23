namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact;

public class UpdateContactRequest
{
    public Guid Id { get; set; }
    public string Salutation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string Phonenumber { get; set; }

    public UpdateContactRequest(Guid id,string salutation, string firstName, string lastName, string displayName, DateTime birthDate,string email, string phonenumber)
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