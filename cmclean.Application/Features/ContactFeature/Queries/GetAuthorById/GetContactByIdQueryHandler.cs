using AutoMapper;
using MediatR;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Interfaces.Repositories.Contacts;

namespace cmclean.Application.Features.ContactFeature.Queries.GetContactById;

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, GetContactByIdResponse>
{
    private readonly IContactReadRepository _ContactReadRepository;
    private readonly IMapper _mapper;

    public GetContactByIdQueryHandler(IContactReadRepository ContactReadRepository,IMapper mapper)
    {
        _ContactReadRepository = ContactReadRepository;
        _mapper = mapper;
    }

    public async Task<GetContactByIdResponse> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var Contact = await _ContactReadRepository.GetByIdAsync(request.ContactId);
        if (Contact is null)
        {
            throw new NotFoundException($"Contact cannot found with id: {request.ContactId}");
        }
        return _mapper.Map<GetContactByIdResponse>(Contact);
    }
}