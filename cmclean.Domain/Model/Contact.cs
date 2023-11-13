using cmclean.Domain.Entities;

namespace cmclean.Domain.Model;
public class  Contact: BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? DisplayName { get; set; }
    //public string? DisplayName
    //{
    //    get { return DisplayName; }
    //    set {
    //        if (value == null)
    //        {
    //            DisplayName = $"{FirstName} {LastName}";
    //        }
    //        else
    //        {
    //            DisplayName = value;
    //        }
    //    }
    //}
    //public Contact()
    //{
    //    Id = Guid.NewGuid();
    //}
   
    public DateTime BirthDate { get; set; }

  

    public void UpdateContact(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = dateOfBirth;
    }
}