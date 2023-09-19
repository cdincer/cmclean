using cmclean.Application.Configuration.Commands;
using cmclean.Domain.Repositories;

namespace cmclean.Application.Contacts.DeleteContact
{
    public class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, ContactDto>
    {
        private readonly IRepositoryWrapper _repo;

        public DeleteContactCommandHandler
        (IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public async Task<ContactDto> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var resultDetail = await _repo.ContactRepository.FindByCondition(c => c.id == request.Id);
            if (resultDetail.Count > 0)
            {
                _repo.ContactRepository.Delete(resultDetail.FirstOrDefault());
                await _repo.Save();
                return new ContactDto { message = "Delete succeeded" }; ;
            }
            return new ContactDto { message = "Delete failed" }; ;
        }
    }
}