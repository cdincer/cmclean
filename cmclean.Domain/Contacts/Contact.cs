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
        private DateTime? _birthdate;
        private DateTime _creationtimestamp;
        private DateTime _lastchangetimestamp;
        private string _email;//Must be unique
        private string? _phonenumber;
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
        public DateTime? birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }
        public DateTime creationtimestamp
        {
            get { return _creationtimestamp; }
            private set { _creationtimestamp = value; }
        }
        public DateTime lastchangetimestamp
        {
            get { return _lastchangetimestamp; }
            private set { _lastchangetimestamp = value; }
        }
        public bool notifyhasbirthdaysoon
        {
            get
            {
                bool birthDayCalc = false;
                if (_birthdate != null)
                {
                    int YearAdjustment = DateTime.Now.Year - _birthdate.Value.Year;
                    DateTime CurrBirthDate = _birthdate.Value.AddYears(YearAdjustment);
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
        public string? phonenumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; }
        }
        #endregion
        public static Contact CreatedRegistered
        (string salutation, string firstname,
        string lastname, string email,
        string displayname, string? phonenumber,
        DateTime? birthdate
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
                , DateTime.Now.ToUniversalTime(), DateTime.Now.ToUniversalTime()
            );
        }

        private Contact
        (
        string salutation, string firstname,
        string lastname, string email,
        string displayname,
        string? phonenumber, DateTime? birthdate
        , DateTime creationtimestamp, DateTime lastchangetimestamp
        )
        {
            id = Guid.NewGuid();
            _salutation = salutation;
            _firstname = firstname;
            _lastname = lastname;
            _email = email;
            _displayname = string.IsNullOrWhiteSpace(displayname) ? salutation + " " + firstname + " " + lastname : displayname;
            _phonenumber = phonenumber;
            _birthdate = birthdate;
            //birthdate == null ? DateTime.MinValue.ToUniversalTime() : birthdate
            _creationtimestamp = creationtimestamp;
            _lastchangetimestamp = lastchangetimestamp;
        }

    }
}