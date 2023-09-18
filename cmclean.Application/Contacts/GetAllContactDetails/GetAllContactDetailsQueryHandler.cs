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
            allcontactlist.ContactList = new List<AllContactDetailsMember>();
            var list = _repo.ContactRepository.FindAll().ToList();
            foreach (var item in list)
            {
                allcontactlist.ContactList.Add(new AllContactDetailsMember()
                {
                    id = item.id,
                    firstname = item.firstname,
                    lastname = item.lastname,
                    email = item.email,
                    displayname = item.displayname,
                    birthdate = item.birthdate,
                    phonenumber = item.phonenumber,
                    salutation = item.salutation,
                    notifyhasbirthdaysoon = item.notifyhasbirthdaysoon,
                    creationtimestamp = item.creationtimestamp,
                    lastchangetimestamp = item.lastchangetimestamp
                });
            }

            return allcontactlist;
        }
    }
}