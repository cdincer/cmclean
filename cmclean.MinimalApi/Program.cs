using cmclean.MinimalApi.Extensions;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = SeriLogger.CustomLoggerConfiguration(builder.Configuration);
builder.Host.UseSerilog();
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
string PostGreConnectionString = configuration["ConnectionStrings:Default"];
builder.Services.SetupTableAndSampleRecords(PostGreConnectionString);
builder.Services.AddServices(builder);
builder.RegisterModules();


var app = builder.Build();
app.ConfigureApplication();
app.MapEndpoints();

app.Run();
