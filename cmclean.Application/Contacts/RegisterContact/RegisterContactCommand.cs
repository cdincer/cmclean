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
        public RegisterContactCommand (RegisterContactCommandRequest request )
        {
            this.salutation = request.salutation;
            this.firstname = request.firstname;
            this.lastname = request.lastname;
            this.email = request.email;
            this.displayname = request.displayname;
            this.birthdate = request.birthdate;
            this.phonenumber = request.phonenumber;
        }

    }
}