using Azure.Messaging.EventGrid;
using Demo.Models;
using Demo.Providers;
using ILogger = Serilog.ILogger;

namespace Demo.Services;

public interface IEventGridService
{
    Task<string> PublishEventAsync(DemoEventMessage eventMessage);
}

public class EventGridService : IEventGridService
{
    private readonly EventGridPublisherClient _eventGridClient;
    private readonly ILogger _logger;

    public EventGridService(IEventGridProvider eventGridProvider, ILogger logger)
    {
        _eventGridClient = eventGridProvider.EventGridPublisherClient;
        _logger = logger.ForContext<EventGridService>();
    }

    public async Task<string> PublishEventAsync(DemoEventMessage eventMessage)
    {
        try
        {
            // Create an EventGridEvent
            var eventGridEvent = new EventGridEvent(
                subject: eventMessage.Subject,
                eventType: "Demo.EventMessage",
                dataVersion: "1.0",
                data: eventMessage
            );

            // Publish the event
            await _eventGridClient.SendEventAsync(eventGridEvent);
            _logger.Information("Event published with id {EventId}", eventMessage.Id);
            return eventMessage.Id;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error publishing event to EventGrid: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
