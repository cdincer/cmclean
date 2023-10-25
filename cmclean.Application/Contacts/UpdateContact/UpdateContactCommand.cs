using cmclean.Application.Configuration.Commands;
using cmclean.Contacts.UpdateContact;

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

        public UpdateContactCommand(UpdateContactRequest UpdateContactDto)
        {
            this.id = UpdateContactDto.id;
            this.salutation = UpdateContactDto.salutation;
            this.firstname = UpdateContactDto.firstname;
            this.lastname = UpdateContactDto.lastname;
            this.email = UpdateContactDto.email;
            this.displayname = UpdateContactDto.displayname;
            this.birthdate = UpdateContactDto.birthdate;
            this.phonenumber = UpdateContactDto.phonenumber;
        }
    }
}