using cmclean.Application.Configuration.Queries;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using MediatR;

namespace cmclean.Application.Contacts.GetAllContactDetails
{
    public class GetAllContactDetailsQueryHandler : IQueryHandler<GetAllContactDetailsQuery, AllContactDetailsDto>
    {
        public Guid Id { get; }
        private readonly IRepositoryWrapper _repo;

        public GetAllContactDetailsQueryHandler(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public async Task<AllContactDetailsDto> Handle(GetAllContactDetailsQuery request, CancellationToken cancellationToken)
        {
            AllContactDetailsDto SingleContact = new();
            List<Contact> SingleContactResults = _repo.ContactRepository.FindAll().ToList();

            SingleContact.ContactList.AddRange((IEnumerable<AllContactDetailsMember>)SingleContactResults);

            return SingleContact;
        }
    }
}