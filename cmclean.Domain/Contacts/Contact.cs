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
        private string? _displayname;
        private DateTime? _birthddate;
        private readonly DateTime _creationTimeStamp;
        private readonly DateTime _lastChangeTimeStamp;
        private bool _notifyHasBirthdaySoon;//14 days limit.
        private string _email;//Must be unique
        private string? _phonenumber;
        #endregion

        #region Property Area
        public string Salutation
        {
            get { return _salutation; }
        }
        public string Firstname
        {
            get { return _firstname; }
        }
        public string Lastname
        {
            get { return _lastname; }
        }
        public string? Displayname
        {
            get { return _displayname; }
        }
        public DateTime? Birthdate
        {
            get { return _birthddate; }
        }
        public DateTime Creationtimestamp
        {
            get { return _creationTimeStamp; }
        }
        public DateTime Lastchangetimestamp
        {
            get { return _lastChangeTimeStamp; }
        }
        public bool Notifyhasbirthdaysoon
        {
            get
            {
                bool birthDayCalc = false;
                if (_birthddate != null)
                {
                    int YearAdjustment = DateTime.Now.Year - _birthddate.Value.Year;
                    DateTime CurrBirthDate = _birthddate.Value.AddYears(YearAdjustment);
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
        }
        public string Phonenumber
        {
            get { return _phonenumber; }
        }
        #endregion
        public static Contact CreatedRegistered
        (string salutation, string firstname,
        string lastname, string email,
        string? displayname = null, DateTime? birthdate = null,
        string? phonenumber = null)
        {
            CheckRule(new PropertyMinLength(salutation));
            CheckRule(new PropertyMinLength(firstname));
            CheckRule(new PropertyMinLength(lastname));
            return new Contact
            (
                salutation, firstname,
                lastname, email, displayname,
                birthdate, phonenumber
            );
        }

        public Contact
        (
        string salutation, string firstname,
        string lastname, string email,
        string? displayname = null, DateTime? birthdate = null,
        string? phonenumber = null
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