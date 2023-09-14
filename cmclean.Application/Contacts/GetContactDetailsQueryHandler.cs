using cmclean.Application.Configuration.Queries;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using MediatR;

namespace cmclean.Application.Contacts
{
    public class GetContactDetailsQueryHandler : IQueryHandler<GetContactDetailsQuery, ContactDetailsDto>
    {
        public Guid Id { get; }
        private readonly IRepositoryWrapper _repo;

        public GetContactDetailsQueryHandler(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public async Task<ContactDetailsDto> Handle(GetContactDetailsQuery request, CancellationToken cancellationToken)
        {
            ContactDetailsDto SingleContact = new();
            List<Contact> SingleContactResults = await _repo.ContactRepository.FindByCondition(c => c.id == request.Id);
            SingleContact.birthdate = SingleContactResults.FirstOrDefault().birthdate;
            SingleContact.creationtimestamp = SingleContactResults.FirstOrDefault().creationtimestamp;
            SingleContact.displayname = SingleContactResults.FirstOrDefault().displayname;
            SingleContact.email = SingleContactResults.FirstOrDefault().email;
            SingleContact.firstname = SingleContactResults.FirstOrDefault().firstname;
            SingleContact.lastname = SingleContactResults.FirstOrDefault().lastname;
            SingleContact.id = SingleContactResults.FirstOrDefault().id;
            SingleContact.lastchangetimestamp = SingleContactResults.FirstOrDefault().lastchangetimestamp;
            SingleContact.phonenumber = SingleContactResults.FirstOrDefault().phonenumber;
            SingleContact.salutation = SingleContactResults.FirstOrDefault().salutation;
            return SingleContact;
        }

    }
}