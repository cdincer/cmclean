using cmclean.Domain.Contacts;

namespace cmclean.Application.Contacts.GetAllContactDetails
{

    public class AllContactDetailsDto
    {
        public List<AllContactDetailsMember> ContactList { get; set; }
    }

    public class AllContactDetailsMember
    {

        public Guid id { get; set; }
        public string salutation { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string displayname { get; set; }
        public DateTime? birthdate { get; set; }
        public DateTime creationtimestamp { get; set; }
        public DateTime lastchangetimestamp { get; set; }
        public bool notifyhasbirthdaysoon { get; set; }//14 days limit.
        public string email { get; set; }//Must be unique
        public string? phonenumber { get; set; }
    }
}