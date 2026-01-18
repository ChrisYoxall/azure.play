using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace Demo.ServiceBus.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(ServiceBusSender sender) : ControllerBase
{
    [HttpGet("Send")]
    public async Task<IActionResult> SendMessage()
    {
        var messageBody = CreateMessage();
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
