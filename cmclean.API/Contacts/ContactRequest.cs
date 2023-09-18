using System.ComponentModel.DataAnnotations;

namespace cmclean.API.Contacts
{
    public class ContactRequest
    {
        public string salutation { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string displayname { get; set; }
        public DateTime? birthdate { get; set; }
        public string? phonenumber { get; set; }
    }
}