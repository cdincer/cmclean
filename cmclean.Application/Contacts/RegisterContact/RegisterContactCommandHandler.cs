using cmclean.Application.Configuration.Commands;
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
                request.displayname, request.birthdate,
                request.phonenumber
            );

            await _repo.ContactRepository.Create(customer);
            return new ContactDto { Id = customer.Id };
        }
    }
}