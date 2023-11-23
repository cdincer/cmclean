using cmclean.Domain.Model;

namespace cmclean.Application.Features.ContactFeature.Commands.DeleteContact;


public class DeleteContactRequest
{
    public Guid Id { get; private set; }
    
    public DeleteContactRequest(Guid id)
    {
        Id = id;
    }
}