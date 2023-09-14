using cmclean.Application.Configuration.Queries;

namespace cmclean.Application.Contacts
{
    public class GetContactDetailsQuery : IQuery<ContactDetailsDto>
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public GetContactDetailsQuery(Guid Id)
        {
            _id = Id;
        }
    }
}