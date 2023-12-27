using AutoMapper;
using MediatR;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using cmclean.Application.Common.Results;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, IDataResult<CreateContactResponse>>
{
    private readonly IMapper _mapper;
    private readonly IContactWriteRepository _ContactWriteRepository;
    private readonly string BlankId = "00000000-0000-0000-0000-000000000000";

    public CreateContactCommandHandler(IContactWriteRepository ContactWriteRepository, IMapper mapper)
    {
        _ContactWriteRepository = ContactWriteRepository;        
        _mapper = mapper;

    }
    public async Task<IDataResult<CreateContactResponse>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        throw new ArgumentException("Contact cannot be null");
        
        var Contact = _mapper.Map<Contact>(request);
        Contact.CreateContact();
        await _ContactWriteRepository.AddAsync(Contact);

        if(Contact.Id.ToString() == BlankId)
        return new ErrorDataResult<CreateContactResponse>($"Unknown error");
        
        var mappedContact = _mapper.Map<CreateContactResponse>(Contact);
        var result = new SuccessDataResult<CreateContactResponse>(mappedContact, "Success");
        return result;
    }
}