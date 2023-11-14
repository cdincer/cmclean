using AutoMapper;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.Interfaces.GrpcServices.ContactGrpc;
using cmclean.Domain.Model;
using cmclean.Infrastructure.Protos;

namespace cmclean.Infrastructure.Services.GrpcServices.ContactGrpc
{
    public class ContactGrpcService : IContactGrpcService
    {

        private readonly ContactProtoService.ContactProtoServiceClient _ContactProtoService;
        private readonly IMapper _mapper;
        public ContactGrpcService(ContactProtoService.ContactProtoServiceClient ContactProtoService, IMapper mapper)
        {
            _ContactProtoService = ContactProtoService;
            _mapper = mapper;
        }

        public async Task<List<GetAllContactsResponse>> GetContactsAsync()
        {
            var result = await _ContactProtoService.GetContactsAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var Contacts = _mapper.Map<List<GetAllContactsResponse>>(result.Contacts);
            return Contacts;
        }
        public async Task<GetContactByIdResponse> GetContactByIdAsync(string ContactID)
        {
            var result = await _ContactProtoService.GetContactByIdAsync(new GetContactByIdProtoRequest { ContactId = ContactID });
            var Contact = _mapper.Map<GetContactByIdResponse>(result.Contact);
            return Contact;
        }
        public async Task<CreateContactResponse> CreateContactAsync(CreateContactRequest Contact)
        {
            try
            {
                var request = _mapper.Map<CreateContactProtoRequest>(Contact);
                var result = await _ContactProtoService.CreateContactAsync(request);
                var addedContact = _mapper.Map<CreateContactResponse>(result.Contact);
                return addedContact;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new CreateContactResponse();
            }
        }
        public async Task<UpdateContactResponse> UpdateContactAsync(UpdateContactRequest Contact, string ContactID)
        {
            var request = _mapper.Map<UpdateContactProtoRequest>(Contact);
            request.Id = ContactID;
            var response = await _ContactProtoService.UpdateContactAsync(request);
            var updatedContact = _mapper.Map<UpdateContactResponse>(response);
            return updatedContact;
        }

        public async Task<DeleteContactResponse> DeleteContactAsync(DeleteContactRequest Contact)
        {
            var request = _mapper.Map<DeleteContactProtoRequest>(Contact);
            var response = await _ContactProtoService.DeleteContactAsync(request);
            var deletedContact = _mapper.Map<DeleteContactResponse>(response);
            return deletedContact;
        }

        public async Task<GetContactByFilterResponse> GetContactByFilterAsync(GetContactByFilterQuery GetContactByFilterQuery)
        {
            var request = _mapper.Map<GetContactByFilterProtoRequest>(GetContactByFilterQuery);

            var result = await _ContactProtoService.GetContactByFilterAsync(request);

            var Contact = _mapper.Map<GetContactByFilterResponse>(result.Contact);
            return Contact;
        }
    }

}
