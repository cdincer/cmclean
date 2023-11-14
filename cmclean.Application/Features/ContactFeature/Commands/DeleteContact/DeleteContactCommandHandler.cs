using MediatR;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Interfaces.Repositories.Contacts;

namespace cmclean.Application.Features.ContactFeature.Commands.DeleteContact;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, DeleteContactResponse>
{
    private readonly IContactWriteRepository _ContactWriteRepository;
    private readonly IContactReadRepository _ContactReadRepository;

    public DeleteContactCommandHandler(IContactWriteRepository ContactWriteRepository, IContactReadRepository ContactReadRepository)
    {
        _ContactWriteRepository = ContactWriteRepository;
        _ContactReadRepository = ContactReadRepository;
    }
    public async Task<DeleteContactResponse> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var Contact = await _ContactReadRepository.GetByIdAsync(request.ContactId);
        if (Contact is null)
            throw new NotFoundException($"Contact cannot found with id: {request.ContactId}");
        
        _ContactWriteRepository.Remove(Contact);
        var result = new DeleteContactResponse
        {
            Status = true
        };
        return result;
    }
}