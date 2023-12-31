﻿using cmclean.GrpcService.Services;
using cmclean.Persistence;
using Consul;
using Microsoft.EntityFrameworkCore;

namespace cmclean.GrpcService.Extensions;

public static class WebApplicationExtensions
{

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MapGrpcService<ContactService>();
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        app.RegisterWithConsul();
        return app;
    }

    private static void RegisterWithConsul(this WebApplication app)
    {
        var consulClient = app.Services.GetRequiredService<IConsulClient>();
        var uri = app.Configuration.GetValue<Uri>("ConsulConfig:ServiceAddress");
        var serviceName = app.Configuration.GetValue<string>("ConsulConfig:ServiceName");
        var serviceId = app.Configuration.GetValue<string>("ConsulConfig:ServiceId");

        var registration = new AgentServiceRegistration()
        {
            ID = serviceId ?? "ThoughtfulApi",
            Name = serviceName ?? "ThoughtfulAApi",
            Address = $"{uri!.Host}",
            Port = uri.Port,
            Tags = new[] { serviceName, serviceId }
        };

        app.Logger.LogInformation("Registering with Consul");
        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        consulClient.Agent.ServiceRegister(registration).Wait();

        app.Lifetime.ApplicationStopping.Register(() =>
        {
            app.Logger.LogInformation("DeRegistering from Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
    }
}
