using System.ComponentModel.DataAnnotations;

namespace cmclean.API.Contacts
{
    public class ContactRequest
    {
        public string Salutation { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string? Displayname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Phonenumber { get; set; }
    }
}