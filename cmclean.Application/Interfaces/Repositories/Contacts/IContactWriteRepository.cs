using cmclean.Domain.Model;

namespace cmclean.Application.Interfaces.Repositories.Contacts;

public interface IContactWriteRepository
{
    Task<Contact> AddAsync(Contact entity);
    Contact Update(Contact entity);
    bool Remove(Contact entity);
}