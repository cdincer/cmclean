using cmclean.Domain.Model;

namespace cmclean.Application.Features.ContactFeature.Commands.DeleteContact;


public class DeleteContactRequest
{
    public Guid ContactId { get; private set; }
    
    public DeleteContactRequest(Guid contactId)
    {
        ContactId = contactId;
    }
}