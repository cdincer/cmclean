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
        public Guid id { get; set; }
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
        public string salutation
        {
            get { return _salutation; }
            set { _salutation = value; }
        }
        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        public string displayname
        {
            get { return _displayname; }
            set { _displayname = value; }
        }
        public DateTime birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }
        public DateTime creationtimestamp
        {
            get { return _creationTimeStamp; }
        }
        public DateTime lastchangetimestamp
        {
            get { return _lastChangeTimeStamp; }
        }
        public bool notifyhasbirthdaysoon
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
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string phonenumber
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
            id = Guid.NewGuid();
            _salutation = salutation;
            _firstname = firstname;
            _lastname = lastname;
            _displayname = string.IsNullOrWhiteSpace(displayname) ? salutation + " " + firstname + " " + lastname : displayname;
            _birthdate = birthdate;
            _creationTimeStamp = DateTime.Now;
            _lastChangeTimeStamp = DateTime.Now;

            if (birthdate != NullCheck)
            {
                int YearAdjustment = DateTime.Now.Year - birthdate.Year;
                DateTime CurrBirthDate = birthdate.AddYears(YearAdjustment);
                DateTime checkBirthDayEndDate = DateTime.Now.AddDays(UserBirthDateCheck);
                if (CurrBirthDate >= DateTime.Now && CurrBirthDate <= checkBirthDayEndDate)
                {
                    _notifyHasBirthdaySoon = true;
                }
            }
            else
            {
                _notifyHasBirthdaySoon = false;
            }
            _email = email;
            _phonenumber = phonenumber;
        }

    }
}