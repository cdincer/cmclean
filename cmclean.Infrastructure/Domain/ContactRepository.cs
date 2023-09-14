using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;

namespace cmclean.Infrastructure.Domain
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(CleanDbContext context) : base(context)
        {
        }
    }
}