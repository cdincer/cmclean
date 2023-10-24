using cmclean.Application.Configuration.Commands;
using cmclean.Application.Contacts.DomainServices;
using cmclean.Application.Contacts.GetAllContactDetails;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace cmclean.Application.Contacts.RegisterContact
{
    public class RegisterContactCommandHandler : ICommandHandler<RegisterContactCommand, ContactDto>
    {
        private readonly IRepositoryWrapper _repo;
        readonly ILogger _log;
        public RegisterContactCommandHandler
        (IRepositoryWrapper repo, ILoggerFactory loggerFactory)
        {
            _repo = repo;
            _log = loggerFactory.CreateLogger(GetType());
        }
        public async Task<ContactDto> Handle(RegisterContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = Contact.CreatedRegistered
                (
                    request.salutation, request.firstname,
                    request.lastname, request.email,
                    request.displayname, request.phonenumber, request.birthdate

                );
                ContactUniquenessChecker check = new(_repo);
                if (await check.IsUnique(request.email) == 0)
                {
                    await _repo.ContactRepository.Create(customer);
                    await _repo.Save();
                    _log.LogInformation("Created contact: " + customer.firstname + " " + customer.lastname);
                    return new ContactDto { id = customer.id };
                }
                _log.LogError("Error handling the register command");
                return new ContactDto { message = "Creation failed" };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error handling the register command");
                return new ContactDto { message = "Creation failed" };
            }
        }
    }
}