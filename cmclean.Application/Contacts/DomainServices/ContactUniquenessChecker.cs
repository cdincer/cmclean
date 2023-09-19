using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;

namespace cmclean.Application.Contacts.DomainServices
{
    public class ContactUniquenessChecker : IContactUniquenessChecker
    {
        private readonly IRepositoryWrapper _repo;

        public ContactUniquenessChecker(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public async Task<int> IsUnique(string contactemail)
        {
            var uniquechecklist = await _repo.ContactRepository.FindByCondition(c => c.email == contactemail);

            return uniquechecklist.Count;
        }
    }
}