using System.ComponentModel.DataAnnotations;

namespace Demo.ServiceBus.Settings;

public class ServiceBusSettings
{
    public const string SectionName = "ServiceBus";
    
    [Required]
    public required string Namespace { get; set; }
    
    [Required]
    public required string TopicName { get; set; }
    }