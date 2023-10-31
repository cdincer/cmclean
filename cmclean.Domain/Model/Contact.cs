using cmclean.Domain.Entities;

namespace cmclean.Domain.Model;
public class  Contact: BaseEntity
{
    //public string? Salutation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? DisplayName => DisplayName ?? $"{FirstName} {LastName}";
    public DateTime BirthDate { get; set; }
    //public string Email { get; set; }
    //public string? PhoneNumber { get; set; }
    //public DateTime CreationTimestamp { get; set; }
    //public DateTime LastchangeTimestamp { get; set; }


    public void UpdateContact(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = dateOfBirth;
    }
}