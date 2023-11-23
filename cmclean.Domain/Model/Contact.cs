using cmclean.Domain.Entities;

namespace cmclean.Domain.Model;
public class Contact: BaseEntity
{
    private readonly int UserBirthDateCheck = 14;
    public string Salutation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreationTimestamp { get; private set; }
    public DateTime LastChangeTimeStamp { get; private set; }
    public bool NotifyHasBirthdaySoon
    {
        get
        {
            bool birthDayCalc = false;
            if (BirthDate.ToString() != "01/01/0001 00:00:00")
            {
                int YearAdjustment = DateTime.Now.Year - BirthDate.Year;
                DateTime CurrBirthDate = BirthDate.AddYears(YearAdjustment);
                DateTime checkBirthDayEndDate = DateTime.Now.AddDays(UserBirthDateCheck);
                if (CurrBirthDate >= DateTime.Now && CurrBirthDate <= checkBirthDayEndDate)
                {
                    birthDayCalc = true;
                }
            }
            return birthDayCalc;
        }

    }
    public string Email { get; set; }
    public string Phonenumber { get; set; }

    public void CreateContact()
    {
        this.Id = Guid.NewGuid();
        this.CreationTimestamp = DateTime.Now;
        this.LastChangeTimeStamp = DateTime.Now;
        this.DisplayName = this.DisplayName.Length != 0 ? DisplayName : Salutation + FirstName + LastName; ;

    }

    public void UpdateContact(string salutation, string firstName, string lastName, 
                              string displayName, DateTime birthDate, string email, string phonenumber)
    {
        Salutation = salutation;
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName.Length != 0 ? displayName : salutation + firstName + lastName; ;
        BirthDate = birthDate;
        LastChangeTimeStamp = DateTime.Now;
        Email = email;
        Phonenumber = phonenumber;
    }
}