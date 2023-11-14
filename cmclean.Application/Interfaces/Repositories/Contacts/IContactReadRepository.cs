using cmclean.Domain.Model;
using System.Linq.Expressions;

namespace cmclean.Application.Interfaces.Repositories.Contacts;

public interface IContactReadRepository
{
    Task<List<Contact>> GetAll();
    Task<Contact?> GetByIdAsync(Guid id);
    Task<Contact?> GetSingleAsync(string filter);
}