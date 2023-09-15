using cmclean.Contacts.Rules;
using cmclean.Domain.SeedWork;

namespace cmclean.Domain.Contacts
{
    //Work In Progess will be used in future classes
    public class Contact : Entity
    {
        private readonly DateTime NullCheck = DateTime.Parse("0001-01-01T00:00:00");
        private readonly int UserBirthDateCheck = 14;
        #region Field Area
        public Guid Id { get; set; }
        private string _salutation;
        private string _firstname;
        private string _lastname;
        private string _displayname;
        private DateTime _birthdate;
        private readonly DateTime _creationTimeStamp;
        private readonly DateTime _lastChangeTimeStamp;
        private bool _notifyHasBirthdaySoon;//14 days limit.
        private string _email;//Must be unique
        private string _phonenumber;
        #endregion

        #region Property Area
        public string Salutation
        {
            get { return _salutation; }
            set { _salutation = value; }
        }
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        public string? Displayname
        {
            get { return _displayname; }
            set { _displayname = value; }
        }
        public DateTime Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }
        public DateTime CreationTimeStamp
        {
            get { return _creationTimeStamp; }
        }
        public DateTime LastChangeTimestamp
        {
            get { return _lastChangeTimeStamp; }
        }
        public bool NotifyHasBirthDaySoon
        {
            get
            {
                bool birthDayCalc = false;
                if (_birthdate != null)
                {
                    int YearAdjustment = DateTime.Now.Year - _birthdate.Year;
                    DateTime CurrBirthDate = _birthdate.AddYears(YearAdjustment);
                    DateTime checkBirthDayEndDate = DateTime.Now.AddDays(UserBirthDateCheck);
                    if (CurrBirthDate >= DateTime.Now && CurrBirthDate <= checkBirthDayEndDate)
                    {
                        birthDayCalc = true;
                    }
                }
                return birthDayCalc;
            }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Phonenumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; }
        }
        #endregion
        public static Contact CreatedRegistered
        (string salutation, string firstname,
        string lastname, string email,
        string displayname, string phonenumber,
        DateTime birthdate
      )
        {
            CheckRule(new PropertyMinLength(salutation));
            CheckRule(new PropertyMinLength(firstname));
            CheckRule(new PropertyMinLength(lastname));
            return new Contact
            (
                salutation, firstname,
                lastname, email, displayname,
                phonenumber, birthdate
            );
        }

        private Contact
        (
        string salutation, string firstname,
        string lastname, string email,
        string displayname,
        string phonenumber, DateTime birthdate
        )
        {
            Id = Guid.NewGuid();
            _salutation = salutation;
            _firstname = firstname;
            _lastname = lastname;
            _displayname = string.IsNullOrWhiteSpace(displayname) ? salutation + " " + firstname + " " + lastname : displayname;
            _birthdate = birthdate;
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