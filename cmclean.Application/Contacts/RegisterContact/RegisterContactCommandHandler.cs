using cmclean.Application.Configuration.Commands;
using cmclean.Application.Contacts.DomainServices;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;

namespace cmclean.Application.Contacts.RegisterContact
{
    public class RegisterContactCommandHandler : ICommandHandler<RegisterContactCommand, ContactDto>
    {
        private readonly IRepositoryWrapper _repo;

        public RegisterContactCommandHandler
        (IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public async Task<ContactDto> Handle(RegisterContactCommand request, CancellationToken cancellationToken)
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
                return new ContactDto { id = customer.id };
            }

            return new ContactDto { message = "Creation failed" }; ;
        }
    }
}