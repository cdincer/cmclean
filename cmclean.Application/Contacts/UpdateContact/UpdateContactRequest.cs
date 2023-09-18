namespace cmclean.Contacts.UpdateContact
{
    public class UpdateContactRequest
    {

        public Guid id { get; set; }
        public string salutation { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string displayname { get; set; }
        public DateTime? birthdate { get; set; }
        public string email { get; set; }//Must be unique
        public string? phonenumber { get; set; }


    }
}