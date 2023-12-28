using MediatR;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;

namespace cmclean.Application.Features.ContactFeature.Commands.UpdateContact;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, IDataResult<UpdateContactResponse>>
{

    private readonly IContactWriteRepository _contactWriteRepository;
    private readonly IContactReadRepository _contactReadRepository;

    public UpdateContactCommandHandler(IContactWriteRepository contactWriteRepository, IContactReadRepository contactReadRepository)
    {
        _contactWriteRepository = contactWriteRepository;
        _contactReadRepository = contactReadRepository;
    }

    public async Task<IDataResult<UpdateContactResponse>> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var Contact = await _contactReadRepository.GetByIdAsync(request.Id);
        if (Contact is null)
            throw new NotFoundException($"Contact cannot found with id: {request.Id}");
        
        Contact.UpdateContact(request.Salutation, request.FirstName,request.LastName, 
                              request.DisplayName, request.BirthDate, request.Email, request.Phonenumber);
        var affected = await _contactWriteRepository.Update(Contact);

        var updateContactResponse = new UpdateContactResponse
        {
            Status = affected
        };
        var result = new SuccessDataResult<UpdateContactResponse>(updateContactResponse, "Success");

        return result;
    }
}