using cmclean.Application.Configuration.Queries;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using MediatR;

namespace cmclean.Application.Contacts.GetContacDetails
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
            List<Contact> SingleContactResults = await _repo.ContactRepository.FindByCondition(c => c.Id == request.Id);

            if (SingleContactResults.Count > 0)
            {
                SingleContact.birthdate = SingleContactResults[0].Birthdate;
                SingleContact.creationtimestamp = SingleContactResults[0].Creationtimestamp;
                SingleContact.displayname = SingleContactResults[0].Displayname;
                SingleContact.email = SingleContactResults[0].Email;
                SingleContact.firstname = SingleContactResults[0].Firstname;
                SingleContact.lastname = SingleContactResults[0].Lastname;
                SingleContact.id = SingleContactResults[0].Id;
                SingleContact.lastchangetimestamp = SingleContactResults[0].Lastchangetimestamp;
                SingleContact.phonenumber = SingleContactResults[0].Phonenumber;
                SingleContact.salutation = SingleContactResults[0].Salutation;
            }

            return SingleContact;
        }

    }
}