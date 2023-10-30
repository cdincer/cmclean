using MediatR;

namespace cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;

public class GetAllContactsQuery : IRequest<List<GetAllContactsResponse>>
{

}