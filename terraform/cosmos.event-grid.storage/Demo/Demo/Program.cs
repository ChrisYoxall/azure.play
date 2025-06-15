using Demo.Providers;
using Demo.Services;
using Demo.Settings;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // Use bootstrap logger initially

try
{
    Log.Information("Starting up");
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Set Serilog as the logging provider
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    AddServices(builder);
    var app = builder.Build();
    ConfigurePipeline(app);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

return;

// Add services to the container
void AddServices(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    
    builder.Services.AddSingleton<ICredentialProvider, CredentialProvider>();
    
    const string cosmosSettingsKey = "CosmosDb";
    AddCosmosDbServices(builder, cosmosSettingsKey);
    
    const string eventGridSettingsKey = "EventGridTopic";
    AddEventGridServices(builder, eventGridSettingsKey);
    
    const string blobStorageSettingsKey = "BlobStorage";
    AddBlobStorageServices(builder, blobStorageSettingsKey);

}

void AddCosmosDbServices(WebApplicationBuilder builder, string cosmosSettingsKey)
{
    var settings = builder.Configuration.GetRequiredSection(cosmosSettingsKey).Get<CosmosSettings>();
    if (settings == null)
    {
        throw new ArgumentNullException(nameof(cosmosSettingsKey), "CosmosDb settings not found");
    }
    builder.Services.AddSingleton(settings);
    builder.Services.AddSingleton<ICosmosClientProvider, CosmosClientProvider>();
    builder.Services.AddScoped<ICosmosDbService, CosmosDbService>();
}

void AddEventGridServices(WebApplicationBuilder builder, string eventGridSettingsKey)
{
    var settings = builder.Configuration.GetRequiredSection(eventGridSettingsKey).Get<EventGridSettings>();
    if (settings == null)
    {
        throw new ArgumentNullException(nameof(eventGridSettingsKey), "EventGrid settings not found");
    }
    builder.Services.AddSingleton(settings);
    builder.Services.AddSingleton<IEventGridProvider, EventGridProvider>();
    builder.Services.AddScoped<IEventGridService, EventGridService>();
}

void AddBlobStorageServices(WebApplicationBuilder builder, string blobStorageSettingsKey)
{
    var settings = builder.Configuration.GetRequiredSection(blobStorageSettingsKey).Get<BlobStorageSettings>();
    if (settings == null)
    {
        throw new ArgumentNullException(nameof(blobStorageSettingsKey), "BlobStorage settings not found");
    }
    builder.Services.AddSingleton(settings);
    builder.Services.AddSingleton<IBlobStorageProvider, BlobStorageProvider>();
    builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
}

// Configure the HTTP request pipeline
void ConfigurePipeline(WebApplication app)
{
    app.UseSerilogRequestLogging();
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }
    
    app.MapControllers();
}