using cmclean.Application.Common.Error.Response;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Interfaces.GrpcServices.ContactGrpc;
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
                .Produces<CreateContactResponse>(201)
                .Produces<ValidationErrorResponse>(StatusCodes.Status422UnprocessableEntity)
                .Produces<ErrorResponse>(400)
                .Produces<ErrorResponse>(500);


            ContactGroup.MapPut("/{id}", UpdateContact)
                 .AddEndpointFilter<GuidValidationFilter>()
                 .WithName("UpdateContact")
                .WithDisplayName("Contact Writes Endpoints")
                .WithTags("Contacts")
                .Produces(204)
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

        private async Task<IResult> CreateContact(CreateContactRequest Contact)
        {
            var addedContact = await _ContactGrpcService.CreateContactAsync(Contact);
            return TypedResults.Ok(addedContact);
        }

        private async Task<IResult> UpdateContact(UpdateContactRequest Contact, string id)
        {
            await _ContactGrpcService.UpdateContactAsync(Contact, id);
            return TypedResults.NoContent();
        }

        private async Task<IResult> DeleteContact(string id)
        {
            var request = new DeleteContactRequest(Guid.Parse(id));
            await _ContactGrpcService.DeleteContactAsync(request);
            return TypedResults.NoContent();
        }
    }
}
