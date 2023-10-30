using AutoMapper;
using MediatR;
using cmclean.Application.Interfaces.Repositories.Contacts;

namespace cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, List<GetAllContactsResponse>>
{
    private readonly IContactReadRepository _authorReadRepository;
    private readonly IMapper _mapper;
    public GetAllContactsQueryHandler(IContactReadRepository authorReadRepository ,IMapper mapper)
    {

        _authorReadRepository = authorReadRepository;       
        _mapper = mapper;

    }

    public async Task<List<GetAllContactsResponse>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _authorReadRepository.GetAll();
        var mappedContacts = _mapper.Map<List<GetAllContactsResponse>>(contacts);
        return mappedContacts;
    }
}