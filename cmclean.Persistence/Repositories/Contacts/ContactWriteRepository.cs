using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;


namespace cmclean.Persistence.Repositories.Contacts
{
    public class ContactWriteRepository : GenericWriteRepository<Contact>, IContactWriteRepository
    {
        public ContactWriteRepository(CmcleanDbContext dbContext) : base(dbContext)
        {
        }
    }
}
