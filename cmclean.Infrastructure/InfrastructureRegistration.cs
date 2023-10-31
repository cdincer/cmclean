using System.Reflection;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using cmclean.Application.Interfaces.GrpcServices.ContactGrpc;
using cmclean.Infrastructure.Services.GrpcServices.ContactGrpc;
using cmclean.Infrastructure.Protos;


namespace cmclean.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddGrpcClient<ContactProtoService.ContactProtoServiceClient>(o =>
        {
            var grpcServiceAddress = configuration["GrpcSettings:ContactGrpcServiceUrl"]!;
            var serviceName = configuration["GrpcSettings:ContactGrpcServiceConsulName"]!;
            var consulClient = services.BuildServiceProvider().GetService<IConsulClient>();
            var allRegisteredServices = consulClient?.Agent.Services().GetAwaiter().GetResult();

            var registeredServices = allRegisteredServices?.Response?.Where(s => s.Key.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();
            if (registeredServices is { Count: > 0 })
            {
                var consulService = registeredServices[0];
                var uriBuilder = new UriBuilder()
                {
                    Host = consulService.Address,
                    Port = consulService.Port
                };
                grpcServiceAddress = uriBuilder.Uri.ToString().TrimEnd('/');
            }
            if (registeredServices != null && registeredServices.Count > 0)
            {
                Console.WriteLine("Count of registered services in here " + registeredServices.Count);
            }
            else
            {
                Console.WriteLine("Registered services are empty");
                Console.WriteLine(grpcServiceAddress + "Address Just Above Line36 findme");
                Console.WriteLine(serviceName + "Port Just Above Line36 findme");
            }
   
            o.Address = new Uri(grpcServiceAddress);

        });
        services.AddScoped<IContactGrpcService, ContactGrpcService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
