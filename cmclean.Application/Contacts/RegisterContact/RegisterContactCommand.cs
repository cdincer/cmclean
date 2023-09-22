using cmclean.Application.Configuration.Commands;

namespace cmclean.Application.Contacts.RegisterContact
{
    public class RegisterContactCommand : CommandBase<ContactDto>
    {
        public string salutation { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string displayname { get; set; }
        public DateTime? birthdate { get; set; }
        public string email { get; set; }//Must be unique
        public string? phonenumber { get; set; }
        public RegisterContactCommand
        (
        string salutation, string firstname,
        string lastname, string email,
        string displayname, DateTime? birthdate,
        string? phonenumber
        )
        {
            this.salutation = salutation;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.displayname = displayname;
            this.birthdate = birthdate;
            this.phonenumber = phonenumber;
        }

    }
}