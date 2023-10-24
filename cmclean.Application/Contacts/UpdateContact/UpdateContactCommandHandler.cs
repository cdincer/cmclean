using cmclean.Application.Configuration.Commands;
using cmclean.Application.Contacts.DomainServices;
using cmclean.Application.Contacts.UpdateContact;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using cmclean.Domain.SeedWork;
using Microsoft.Extensions.Logging;

namespace cmclean.Application.Contacts.RegisterContact
{
    public class UpdateContactCommandHandler : ICommandHandler<UpdateContactCommand, ContactDto>
    {
        private readonly IRepositoryWrapper _repo;
        readonly ILogger _log;

        public UpdateContactCommandHandler
        (IRepositoryWrapper repo, ILoggerFactory loggerFactory)
        {
            _repo = repo;
            _log = loggerFactory.CreateLogger(GetType());
        }
        public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {

            try
            {
                ContactUniquenessChecker check = new(_repo);
                var updatecontactresult = await _repo.ContactRepository.FindByCondition(c => c.id == request.id);

                if (updatecontactresult != null && request.email == updatecontactresult.FirstOrDefault().email)
                {
                    UpdateContact(request, updatecontactresult);
                    await _repo.Save();
                    return new ContactDto { id = request.id };
                }
                else if (updatecontactresult.Count > 0 && request.email != updatecontactresult.FirstOrDefault().email)
                    if (await check.IsUnique(request.email) == 0)
                    {
                        UpdateContact(request, updatecontactresult);
                        await _repo.Save();
                        return new ContactDto { id = request.id };
                    }

                return new ContactDto { message = "Update contact failed" }; ;
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error handling the update command");
                return new ContactDto { message = "Update contact failed" }; ;
            }
        }

        private static void UpdateContact(UpdateContactCommand request, List<Contact>? updatecontactresult)
        {
            var contupdate = updatecontactresult.FirstOrDefault();
            contupdate.ChangeContact(request.salutation, request.firstname, request.lastname);
            contupdate.firstname = request.firstname;
            contupdate.lastname = request.lastname;
            contupdate.email = request.email;
            contupdate.displayname = request.displayname;
            contupdate.birthdate = request.birthdate;
            contupdate.phonenumber = request.phonenumber;
            contupdate.salutation = request.salutation;
        }
    }
}