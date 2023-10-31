using MediatR;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Interfaces.Repositories.Contacts;

namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, UpdateContactResponse>
{

    private readonly IContactWriteRepository _contactWriteRepository;
    private readonly IContactReadRepository _contactReadRepository;

    public UpdateContactCommandHandler(IContactWriteRepository contactWriteRepository, IContactReadRepository contactReadRepository)
    {
        _contactWriteRepository = contactWriteRepository;
        _contactReadRepository = contactReadRepository;
    }

    public async Task<UpdateContactResponse> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var Contact = await _contactReadRepository.GetByIdAsync(request.ContactId);
        if (Contact is null)
            throw new NotFoundException($"Contact cannot found with id: {request.ContactId}");
        
        Contact.UpdateContact(request.FirstName, request.LastName, request.DateOfBirth);
        await _contactWriteRepository.SaveChangesAsync();
        var result = new UpdateContactResponse
        {
            Status = true
        };
        return result;
    }
}