using cmclean.Domain.Entities;

namespace cmclean.Domain.Model;
public class Contact: BaseEntity
{
    public string Salutation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? DisplayName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime LastChangeTimeStamp { get; set; }
    public bool NotifyHasBirthdaySoon { get; set; }
    public string Email { get; set; }
    public string Phonenumber { get; set; }


    public void CreateContact()
    {
        this.Id = Guid.NewGuid();
        this.CreationTimestamp = DateTime.Now;
        this.LastChangeTimeStamp = DateTime.Now;
    }

  

    public void UpdateContact(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}