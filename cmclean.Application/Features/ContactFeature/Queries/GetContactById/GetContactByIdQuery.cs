using MediatR;

namespace cmclean.Application.Features.ContactFeature.Queries.GetContactById;

public class GetContactByIdQuery : IRequest<GetContactByIdResponse>
{
    public Guid ContactId { get; private set; }
    public GetContactByIdQuery(Guid contactId)
    {
        ContactId = contactId;
    }
}