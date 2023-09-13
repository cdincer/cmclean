using System.ComponentModel.DataAnnotations;

namespace cmclean.API.Contacts
{
    public class ContactRequest
    {
        public string Salutation { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string Email { get; set; }
        string? Displayname { get; set; }
        DateTime? Birthdate { get; set; }
        string? PhoneNumber { get; set; }
    }
}