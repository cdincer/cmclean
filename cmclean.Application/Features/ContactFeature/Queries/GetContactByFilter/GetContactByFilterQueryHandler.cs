using AutoMapper;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.Interfaces.Repositories.Contacts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter
{
    public class GetContactByFilterQueryHandler : IRequestHandler<GetContactByFilterQuery, GetContactByFilterResponse>
    {

        private readonly IContactReadRepository _ContactReadRepository;
        private readonly IMapper _mapper;


        public GetContactByFilterQueryHandler(IContactReadRepository contactReadRepository, IMapper mapper)
        {
            _ContactReadRepository = contactReadRepository;
            _mapper = mapper;
        }

        public async Task<GetContactByFilterResponse> Handle(GetContactByFilterQuery request, CancellationToken cancellationToken)
        {
            var Contact = await _ContactReadRepository.GetAsync(request);
            if (Contact is null)
            {
                throw new NotFoundException($"Contact cannot found with sent in parameters: {request.FirstName}, {request.LastName}, {request.DateOfBirth}");
            }
            return _mapper.Map<GetContactByFilterResponse>(Contact);
        }
    }
}
