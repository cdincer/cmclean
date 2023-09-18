using cmclean.Application.Configuration.Commands;

namespace cmclean.Application.Contacts.UpdateContact
{
    public class UpdateContactCommand : CommandBase<ContactDto>
    {
        public Guid id { get; set; }
        public string salutation { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string displayname { get; set; }
        public DateTime? birthdate { get; set; }
        public string email { get; set; }//Must be unique
        public string? phonenumber { get; set; }

        public UpdateContactCommand
        (Guid id,
        string salutation, string firstname,
        string lastname, string email,
        string displayname, DateTime? birthdate,
        string? phonenumber
        )
        {
            this.id = id;
            this.salutation = salutation;
            this.firstname = firstname;
            this.lastname = lastname;
            this.displayname = displayname;
            this.birthdate = birthdate;
            this.email = email;
            this.phonenumber = phonenumber;
        }
    }
}