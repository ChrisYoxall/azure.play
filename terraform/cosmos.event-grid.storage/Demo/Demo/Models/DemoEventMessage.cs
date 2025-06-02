namespace Demo.Models;

public class DemoEventMessage
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Subject { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
