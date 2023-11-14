﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using cmclean.LogConsumer.ElasticContexts;
using cmclean.LogConsumer.Models;

namespace cmclean.LogConsumer;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", reloadOnChange: false, optional: false)
                    .AddEnvironmentVariables()
                    .Build();

                services.Configure<RabbitMqConfigModel>(configuration.GetSection("RabbitMqConfig"));
                services.Configure<ElasticSearchConfigModel>(configuration.GetSection("ElasticSearchConfig"));
                services.AddSingleton<IElasticContext, ElasticContext>();
                services.AddHostedService<ConsumerBackgroundService>();
            });
}
