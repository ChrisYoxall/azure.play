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
            {
                try
                {
                    ServiceBusConnectionStringProperties.Parse(settings.ConnectionString);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }, "The provided Service Bus connection string is invalid.")
            .ValidateOnStart();

        // Singleton ServiceBusClient to avoid recreating the client for each request
        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
            return new ServiceBusClient(settings.ConnectionString);
        });

        // Singleton sender to avoid the overhead of establishing a new AMQP link for every message.
        builder.Services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<ServiceBusClient>();
            var settings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
            return client.CreateSender(settings.TopicName);
        });
    }
}