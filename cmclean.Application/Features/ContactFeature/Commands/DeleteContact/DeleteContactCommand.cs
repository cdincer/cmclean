using cmclean.Application.Common.Results;
using MediatR;

namespace cmclean.Application.Features.ContactFeature.Commands.DeleteContact;

public class DeleteContactCommand : IRequest<IDataResult<DeleteContactResponse>>
{
    public Guid Id { get; private set; }
    
    public DeleteContactCommand(Guid id)
    {
        Id = id;
    }
}
