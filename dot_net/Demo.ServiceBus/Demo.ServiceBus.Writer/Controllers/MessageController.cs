using Azure.Messaging.ServiceBus;
using Demo.ServiceBus.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Demo.ServiceBus.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(ServiceBusClient client, IOptions<ServiceBusSettings> serviceBusSettings)
    : ControllerBase
{
    private readonly ServiceBusSettings _serviceBusSettings = serviceBusSettings.Value;

    [HttpGet("Send")]
    public async Task<IActionResult> SendMessage()
    {
        var messageBody = CreateMessage();

        var sender = client.CreateSender(_serviceBusSettings.TopicName);
        var message = new ServiceBusMessage(messageBody);

        try
        {
            await sender.SendMessageAsync(message);
            return Ok($"Message sent: {messageBody}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending message: {ex.Message}");
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }

    private static string CreateMessage()
    {
        var payload = new
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow
        };
        var messageBody = System.Text.Json.JsonSerializer.Serialize(payload);
        return messageBody;
    }
}
