using cmclean.Application.Configuration.Queries;
using cmclean.Application.Contacts.GetContacDetails;
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
            AllContactDetailsDto allcontactlist = new();
            List<Contact> getallcontactresults = _repo.ContactRepository.FindAll().ToList();

            allcontactlist.ContactList = new List<AllContactDetailsMember>();
            foreach (var item in getallcontactresults)
            {
                allcontactlist.ContactList.Add(new AllContactDetailsMember()
                {
                    id = item.id,
                    firstname = item.firstname,
                    lastname = item.lastname,
                    email = item.email,
                    displayname = item.displayname,
                    phonenumber = item.phonenumber,
                    salutation = item.salutation,
                    birthdate = item.birthdate

                });
            }

            return allcontactlist;
        }
    }
}