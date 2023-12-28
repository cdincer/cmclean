using cmclean.Application.Common.Error.Response;
using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Interfaces.GrpcServices.ContactGrpc;
using cmclean.Domain.Model;
using cmclean.MinimalApi.Abstractions;
using cmclean.MinimalApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cmclean.MinimalApi.Features.ContactFeature.Endpoints
{
    public class ContactWritesEndpoint : IEndpoint
    {
        private readonly IContactGrpcService _ContactGrpcService;

        public ContactWritesEndpoint(IContactGrpcService ContactGrpcService)
        {
            _ContactGrpcService = ContactGrpcService;
        }

        public IEndpointRouteBuilder RegisterRoute(IEndpointRouteBuilder endpoints)
        {
            var ContactGroup = endpoints.MapGroup("/api/contacts").AddEndpointFilter<ApiExceptionFilter>();

            ContactGroup.MapPost("/", CreateContact)
                .WithName("CreateContacts")
                .WithDisplayName("Contact Writes Endpoints")
                .WithTags("Contacts")
                .Produces<IDataResult<CreateContactResponse>>(201)
                .Produces<ValidationErrorResponse>(StatusCodes.Status422UnprocessableEntity)
                .Produces<ErrorResponse>(400)
                .Produces<ErrorResponse>(500);


            ContactGroup.MapPut("/", UpdateContact)
                 .AddEndpointFilter<GuidValidationFilter>()
                 .WithName("UpdateContact")
                .WithDisplayName("Contact Writes Endpoints")
                .WithTags("Contacts")
                .Produces<IDataResult<UpdateContactResponse>>(204)
                .Produces<ValidationErrorResponse>(StatusCodes.Status422UnprocessableEntity)
                .Produces<ErrorResponse>(400)
                .Produces<ErrorResponse>(500);

            ContactGroup.MapDelete("/{id}", DeleteContact)
                .AddEndpointFilter<GuidValidationFilter>()
                .WithName("DeleteContact")
                .WithDisplayName("Contact Writes Endpoints")
                .WithTags("Contacts")
                .Produces(204)
                .Produces<ErrorResponse>(500);

            return ContactGroup;
        }

        private async Task<IDataResult<CreateContactResponse>> CreateContact(CreateContactRequest Contact)
        {
            var addedContact = await _ContactGrpcService.CreateContactAsync(Contact);
            if (addedContact.Success)
                return new SuccessDataResult<CreateContactResponse>(addedContact.Data, addedContact.Message);

            return new ErrorDataResult<CreateContactResponse>(new CreateContactResponse(), addedContact.Message);
        }

        private async Task<IDataResult<UpdateContactResponse>> UpdateContact(UpdateContactRequest Contact)
        {
            var updatedContact = await _ContactGrpcService.UpdateContactAsync(Contact);
            if (updatedContact.Success)
                return new SuccessDataResult<UpdateContactResponse>(updatedContact.Data, updatedContact.Message);

            return new ErrorDataResult<UpdateContactResponse>(new UpdateContactResponse(), updatedContact.Message);
        }

        private async Task<IDataResult<DeleteContactResponse>> DeleteContact(string id)
        {
            var request = new DeleteContactRequest(Guid.Parse(id));
            var deletedContact = await _ContactGrpcService.DeleteContactAsync(request);

            if (deletedContact.Success)
                return new SuccessDataResult<DeleteContactResponse>(deletedContact.Data,deletedContact.Message);

            return new ErrorDataResult<DeleteContactResponse>(new DeleteContactResponse(), deletedContact.Message);
        }
    }
}
