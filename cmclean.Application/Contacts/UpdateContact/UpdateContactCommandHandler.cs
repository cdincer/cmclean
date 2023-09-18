using cmclean.Application.Configuration.Commands;
using cmclean.Application.Contacts.DomainServices;
using cmclean.Application.Contacts.UpdateContact;
using cmclean.Domain.Contacts;
using cmclean.Domain.Repositories;
using cmclean.Domain.SeedWork;

namespace cmclean.Application.Contacts.RegisterContact
{
    public class UpdateContactCommandHandler : ICommandHandler<UpdateContactCommand, ContactDto>
    {
        private readonly IRepositoryWrapper _repo;

        public UpdateContactCommandHandler
        (IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {

            // ContactUniquenessChecker check = new(_repo);
            // if (await check.IsUnique(request.email))
            // {

            // }
            Guid tolookfor = request.id;
            var updatecontactresult = await _repo.ContactRepository.FindByCondition(c => c.email == request.email);
            var contupdate = updatecontactresult.FirstOrDefault();
            contupdate.firstname = request.firstname;
            contupdate.lastname = request.lastname;
            contupdate.email = request.email;
            contupdate.displayname = request.displayname;
            contupdate.birthdate = request.birthdate;
            contupdate.phonenumber = request.phonenumber;
            contupdate.salutation = request.salutation;

            await _repo.Save();

            return null;
        }
    }
}