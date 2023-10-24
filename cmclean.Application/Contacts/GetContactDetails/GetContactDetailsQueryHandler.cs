using cmclean.Application.Configuration.Queries;
using cmclean.Application.Contacts.GetAllContactDetails;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace cmclean.Application.Contacts.GetContacDetails
{
    public class GetContactDetailsQueryHandler : IQueryHandler<GetContactDetailsQuery, ContactDetailsDto>
    {
        public Guid Id { get; }
        private readonly IRepositoryWrapper _repo;
        readonly ILogger _log;


        public GetContactDetailsQueryHandler(IRepositoryWrapper repo, ILoggerFactory loggerFactory)
        {
            _repo = repo;
            _log = loggerFactory.CreateLogger(GetType());
        }

        public async Task<ContactDetailsDto> Handle(GetContactDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                ContactDetailsDto SingleContact = new();
                List<Contact> SingleContactResults = await _repo.ContactRepository.FindByCondition(c => c.id == request.Id);

                if (SingleContactResults.Count > 0)
                {
                    SingleContact.birthdate = SingleContactResults[0].birthdate;
                    SingleContact.creationtimestamp = SingleContactResults[0].creationtimestamp;
                    SingleContact.displayname = SingleContactResults[0].displayname;
                    SingleContact.email = SingleContactResults[0].email;
                    SingleContact.firstname = SingleContactResults[0].firstname;
                    SingleContact.lastname = SingleContactResults[0].lastname;
                    SingleContact.id = SingleContactResults[0].id;
                    SingleContact.lastchangetimestamp = SingleContactResults[0].lastchangetimestamp;
                    SingleContact.phonenumber = SingleContactResults[0].phonenumber;
                    SingleContact.salutation = SingleContactResults[0].salutation;
                }
                _log.LogInformation("Found contact : " + SingleContactResults[0].firstname +" "+ SingleContactResults[0].lastname);
                return SingleContact;

            }
            catch (Exception e)
            {
                _log.LogError(e, "Error handling the getsingle command");
                ContactDetailsDto SingleContact = new();
                return SingleContact;
            }
        }
    }
    }