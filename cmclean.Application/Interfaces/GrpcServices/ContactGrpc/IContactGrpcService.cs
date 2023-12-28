using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;

namespace cmclean.Application.Interfaces.GrpcServices.ContactGrpc
{
    public interface IContactGrpcService
    {
        Task<List<GetAllContactsResponse>> GetContactsAsync();
        Task<GetContactByIdResponse> GetContactByIdAsync(string ContactID);
        Task<List<GetContactByFilterResponse>> GetContactByFilterAsync(GetContactByFilterQuery GetContactByFilterQuery);
        Task<IDataResult<CreateContactResponse>> CreateContactAsync(CreateContactRequest Contact);
        Task<IDataResult<UpdateContactResponse>> UpdateContactAsync(UpdateContactRequest Contact);
        Task<DeleteContactResponse> DeleteContactAsync(DeleteContactRequest Contact);
    }
}
