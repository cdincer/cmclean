using AutoMapper;
using MediatR;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, CreateContactResponse>
{
    private readonly IMapper _mapper;
    private readonly IContactWriteRepository _ContactWriteRepository;

    public CreateContactCommandHandler(IContactWriteRepository ContactWriteRepository, IMapper mapper)
    {
        _ContactWriteRepository = ContactWriteRepository;        
        _mapper = mapper;

    }
    public async Task<CreateContactResponse> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentException("Contact cannot be null");
        }
        var Contact = _mapper.Map<Contact>(request);

        await _ContactWriteRepository.AddAsync(Contact);
        await _ContactWriteRepository.SaveChangesAsync();

        var mappedContact = _mapper.Map<CreateContactResponse>(Contact);
        return mappedContact;
    }
}