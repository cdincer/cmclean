using AutoMapper;
using MediatR;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Interfaces.Repositories.Contacts;

namespace cmclean.Application.Features.ContactFeature.Queries.GetContactById;

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, GetContactByIdResponse>
{
    private readonly IContactReadRepository _authorReadRepository;
    private readonly IMapper _mapper;

    public GetContactByIdQueryHandler(IContactReadRepository authorReadRepository,IMapper mapper)
    {
        _authorReadRepository = authorReadRepository;
        _mapper = mapper;
    }

    public async Task<GetContactByIdResponse> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _authorReadRepository.GetByIdAsync(request.ContactId);
        if (author is null)
        {
            throw new NotFoundException($"Author cannot found with id: {request.ContactId}");
        }
        return _mapper.Map<GetContactByIdResponse>(author);
    }
}