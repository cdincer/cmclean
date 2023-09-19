using System.Windows.Input;
using cmclean.Application.Configuration.Commands;
using cmclean.Domain.Contacts;

namespace cmclean.Application.Contacts.DeleteContact
{
    public class DeleteContactCommand : CommandBase<ContactDto>
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public DeleteContactCommand(Guid Id)
        {
            _id = Id;
        }
    }
}