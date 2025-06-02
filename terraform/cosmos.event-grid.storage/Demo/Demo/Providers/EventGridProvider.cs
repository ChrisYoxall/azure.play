using Azure.Messaging.EventGrid;
using Demo.Settings;

namespace Demo.Providers;

public interface IEventGridProvider
{
    EventGridPublisherClient EventGridPublisherClient { get; init; }
}

public class EventGridProvider : IEventGridProvider
{
    public EventGridPublisherClient EventGridPublisherClient { get; init; }
    
    public EventGridProvider(ICredentialProvider credentialProvider, EventGridSettings settings)
    {
        EventGridPublisherClient = new EventGridPublisherClient(new Uri(settings.Endpoint!), credentialProvider.GetCredential());
    }
}