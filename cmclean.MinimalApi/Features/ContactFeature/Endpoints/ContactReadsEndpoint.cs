using cmclean.Application.Common.Error.Response;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.Interfaces.GrpcServices.ContactGrpc;
using cmclean.MinimalApi.Abstractions;
using cmclean.MinimalApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cmclean.MinimalApi.Features.ContactFeature.Endpoints
{
    public class ContactReadsEndpoint : IEndpoint
    {
        private readonly IContactGrpcService _ContactGrpcService;

        public ContactReadsEndpoint(IContactGrpcService ContactGrpcService)
        {
            _ContactGrpcService = ContactGrpcService;
        }

        public IEndpointRouteBuilder RegisterRoute(IEndpointRouteBuilder endpoints)
        {
            var ContactGroup = endpoints.MapGroup("/api/Contacts").AddEndpointFilter<ApiExceptionFilter>();

            ContactGroup.MapGet("/", GetAllContacts)
                .WithName("GetAllContacts")
                .WithDisplayName("Contact Reads Endpoints")
                .WithTags("Contacts")
                .Produces<List<GetAllContactsResponse>>()
                .Produces(500);

            ContactGroup.MapGet("/{id}", GetContactById)
                .AddEndpointFilter<GuidValidationFilter>()
                .WithName("GetContactById")
                .WithDisplayName("Contact Reads Endpoints")
                .WithTags("Contacts")
                .Produces<GetContactByIdResponse>(200)
                .Produces<ErrorResponse>(404)
                .Produces<ErrorResponse>(500);


            return ContactGroup;

        }

        private async Task<IResult> GetAllContacts()
        {
            var Contacts = await _ContactGrpcService.GetContactsAsync();
            return Results.Ok(Contacts);
        }


        private async Task<IResult> GetContactById(string id)
        {
            var Contact = await _ContactGrpcService.GetContactByIdAsync(id);
            return TypedResults.Ok(Contact);
        }

    }
}
