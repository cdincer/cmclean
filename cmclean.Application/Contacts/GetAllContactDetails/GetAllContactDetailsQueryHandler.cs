using cmclean.Application.Configuration.Queries;
using cmclean.Application.Contacts.GetContacDetails;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace cmclean.Application.Contacts.GetAllContactDetails
{
    public class GetAllContactDetailsQueryHandler : IQueryHandler<GetAllContactDetailsQuery, AllContactDetailsDto>
    {
        public Guid Id { get; }
        private readonly IRepositoryWrapper _repo;
        readonly ILogger _log;
        public GetAllContactDetailsQueryHandler(IRepositoryWrapper repo, ILoggerFactory loggerFactory)
        {
            _repo = repo;
            _log = loggerFactory.CreateLogger(GetType());
        }

        public async Task<AllContactDetailsDto> Handle(GetAllContactDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                AllContactDetailsDto allcontactlist = new()
                {
                    ContactList = new()
                };
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
                _log.LogInformation("Returned amount of contacts : " + list.Count);
                return allcontactlist;
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error handling the getall command");
                AllContactDetailsDto allcontactlist = new()
                {
                    ContactList = new()
                };
                return allcontactlist;
            }
        }
    }
}