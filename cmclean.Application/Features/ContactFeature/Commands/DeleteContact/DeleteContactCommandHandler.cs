using MediatR;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;

namespace cmclean.Application.Features.ContactFeature.Commands.DeleteContact;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, IDataResult<DeleteContactResponse>>
{
    private readonly IContactWriteRepository _ContactWriteRepository;
    private readonly IContactReadRepository _ContactReadRepository;

    public DeleteContactCommandHandler(IContactWriteRepository ContactWriteRepository, IContactReadRepository ContactReadRepository)
    {
        _ContactWriteRepository = ContactWriteRepository;
        _ContactReadRepository = ContactReadRepository;
    }
    public async Task<IDataResult<DeleteContactResponse>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var Contact = await _ContactReadRepository.GetByIdAsync(request.Id);
        if (Contact is null)
            throw new NotFoundException($"Contact cannot found with id: {request.Id}");
        
        var affected = await _ContactWriteRepository.Remove(Contact);
        var deletedContactResponse = new DeleteContactResponse
        {
            Status = affected
        };

        var result = new SuccessDataResult<DeleteContactResponse>(deletedContactResponse, "Success");

        return result;
    }
}