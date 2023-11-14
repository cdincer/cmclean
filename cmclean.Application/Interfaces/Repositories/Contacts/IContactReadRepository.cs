using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Domain.Model;
using System.Linq.Expressions;

namespace cmclean.Application.Interfaces.Repositories.Contacts;

public interface IContactReadRepository
{
    Task<List<Contact>> GetAll();
    Task<Contact?> GetByIdAsync(Guid id);
    Task<Contact?> GetAsync(GetContactByFilterQuery getContactByFilterQuery);
}