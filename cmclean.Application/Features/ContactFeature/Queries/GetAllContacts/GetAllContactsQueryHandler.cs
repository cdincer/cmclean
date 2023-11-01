using AutoMapper;
using MediatR;
using cmclean.Application.Interfaces.Repositories.Contacts;

namespace cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, List<GetAllContactsResponse>>
{
    private readonly IContactReadRepository _ContactReadRepository;
    private readonly IMapper _mapper;
    public GetAllContactsQueryHandler(IContactReadRepository ContactReadRepository ,IMapper mapper)
    {

        _ContactReadRepository = ContactReadRepository;       
        _mapper = mapper;

    }

    public async Task<List<GetAllContactsResponse>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _ContactReadRepository.GetAll();
        var mappedContacts = _mapper.Map<List<GetAllContactsResponse>>(contacts);
        return mappedContacts;
    }
}