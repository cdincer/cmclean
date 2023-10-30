using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Persistence.Repositories.Contacts;

public class ContactReadRepository : GenericReadRepository<Contact>, IContactReadRepository
{
    public ContactReadRepository(CmcleanDbContext dbContext) : base(dbContext)
    {

    }
}
