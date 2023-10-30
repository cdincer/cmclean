using MediatR;

namespace cmclean.Application.Features.ContactFeature.Commands.DeleteContact;

public class DeleteContactCommand : IRequest<DeleteContactResponse>
{
    public Guid ContactId { get; private set; }
    
    public DeleteContactCommand(Guid contactId)
    {
        ContactId = contactId;
    }
}
