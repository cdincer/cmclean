using cmclean.Application.Interfaces.GrpcServices.ContactGrpc;
using cmclean.MinimalApi.Abstractions;
using cmclean.MinimalApi.Features.ContactFeature.Endpoints;

namespace cmclean.MinimalApi.Features.ContactFeature
{
    public class ContactModule : IModule
    {
        private IContactGrpcService? _ContactGrpcService;
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            new ContactReadsEndpoint(_ContactGrpcService!).RegisterRoute(endpoints);
            new ContactWritesEndpoint(_ContactGrpcService!).RegisterRoute(endpoints);
            return endpoints;
        }

        public WebApplicationBuilder RegisterModule(WebApplicationBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();
            _ContactGrpcService = provider.GetRequiredService<IContactGrpcService>();
            return builder;
        }
    }
}
