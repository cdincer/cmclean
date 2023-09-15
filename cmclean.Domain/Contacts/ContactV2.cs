using cmclean.Contacts.Rules;
using cmclean.Domain.SeedWork;

namespace cmclean.Domain.ContactV2s
{
    //Work In Progess will be used in future classes
    public class ContactV2 : Entity
    {
        private readonly DateTime NullCheck = DateTime.Parse("0001-01-01T00:00:00");
        private readonly int UserBirthDateCheck = 14;

        #region Field Area
        public Guid Id { get; private set; }
        private string _salutation;
        private string _firstname;
        private string _lastname;
        private string _displayname;
        private DateTime _birthddate;
        private readonly DateTime _creationTimeStamp;
        private readonly DateTime _lastChangeTimeStamp;
        private bool _notifyHasBirthdaySoon;//14 days limit.
        private string _email;//Must be unique
        private string _phonenumber;
        #endregion

        public static ContactV2 CreatedRegistered
        (string salutation, string firstname,
        string lastname, string email,
        string displayname, DateTime birthdate,
        string phonenumber)
        {
            CheckRule(new PropertyMinLength(salutation));
            CheckRule(new PropertyMinLength(firstname));
            CheckRule(new PropertyMinLength(lastname));
            return new ContactV2
            (
                salutation, firstname,
                lastname, email, displayname,
                birthdate, phonenumber
            );
        }


        private ContactV2
        (
        string salutation, string firstname,
        string lastname, string email,
        string displayname, DateTime birthdate,
        string phonenumber
        )
        {
            Id = Guid.NewGuid();
            _salutation = salutation;
            _firstname = firstname;
            _lastname = lastname;
            _displayname = string.IsNullOrWhiteSpace(displayname) ? salutation + " " + firstname + " " + lastname : displayname;
            _birthddate = birthdate;
            _creationTimeStamp = DateTime.Now;
            _lastChangeTimeStamp = DateTime.Now;

            if (birthdate != NullCheck)
            {
                bool birthDayCalc = false;
                DateTime checkBirthDayEndDate = DateTime.Now.AddDays(UserBirthDateCheck); ;
                if (birthdate >= DateTime.Now && birthdate <= checkBirthDayEndDate)
                {
                    birthDayCalc = true;
                }
                _notifyHasBirthdaySoon = birthDayCalc;
            }
            _email = email;
            _phonenumber = phonenumber;
        }
    }
}