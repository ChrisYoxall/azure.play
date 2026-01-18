using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Demo.ServiceBus.Settings;
using Microsoft.Extensions.Options;

namespace Demo.ServiceBus;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        AddServiceBus(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapControllers();
        
        app.Run();
    }

    private static void AddServiceBus(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<ServiceBusSettings>()
            .BindConfiguration(ServiceBusSettings.SectionName)
            .ValidateDataAnnotations()
            .Validate(settings => 
                    !string.IsNullOrWhiteSpace(settings.Namespace) && Uri.CheckHostName(settings.Namespace) != UriHostNameType.Unknown, 
                "The provided Service Bus Namespace is invalid.")
            .ValidateOnStart();

        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
            return new ServiceBusClient(settings.Namespace, new DefaultAzureCredential());
        });

        builder.Services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<ServiceBusClient>();
            var settings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
            return client.CreateSender(settings.TopicName);
        });
    }
}