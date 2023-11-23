using cmclean.Domain.Model;

namespace cmclean.Application.Interfaces.Repositories.Contacts;

public interface IContactWriteRepository
{
    Task<Contact> AddAsync(Contact entity);
    Task<bool> Update(Contact entity);
    Task<bool> Remove(Contact entity);
}