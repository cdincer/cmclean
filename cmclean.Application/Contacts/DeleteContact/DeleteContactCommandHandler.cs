using cmclean.Application.Configuration.Commands;
using cmclean.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cmclean.Application.Contacts.DeleteContact
{
    public class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, ContactDto>
    {
        private readonly IRepositoryWrapper _repo;
        readonly ILogger _log;

        public DeleteContactCommandHandler
        (IRepositoryWrapper repo, ILoggerFactory loggerFactory)
        {
            _repo = repo;
            _log = loggerFactory.CreateLogger(GetType());
        }
        public async Task<ContactDto> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resultDetail = await _repo.ContactRepository.FindByCondition(c => c.id == request.Id);
                if (resultDetail.Count > 0)
                {
                    _repo.ContactRepository.Delete(resultDetail.FirstOrDefault());
                    await _repo.Save();
                    return new ContactDto { message = "Delete succeeded" };
                }
                return new ContactDto { message = "Delete failed" };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error handling the delete command");
                return new ContactDto { message = "Error handling delete command" };
            }
        }
    }
}