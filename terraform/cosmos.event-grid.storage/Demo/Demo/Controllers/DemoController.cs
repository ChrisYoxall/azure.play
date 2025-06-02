using Microsoft.AspNetCore.Mvc;
using Demo.Models;
using Demo.Services;
using ILogger = Serilog.ILogger;

namespace Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IEventGridService _eventGridService;

    public DemoController(ILogger logger, ICosmosDbService cosmosDbService, IEventGridService eventGridService)
    {
        _logger = logger.ForContext<DemoController>();
        _cosmosDbService = cosmosDbService;
        _eventGridService = eventGridService;
    }

    [HttpGet("cosmosdb")]
    public async Task<IActionResult> CosmosDb()
    {

        try
        {
            // Create a demo document
            var demoDocument = new DemoDocument
            {
                Name = $"Demo Document {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                Description = "This is a test document created for CosmosDB demo"
            };

            // Save the document to CosmosDB, then read it back
            var documentId = await _cosmosDbService.CreateDocumentAsync(demoDocument);
            var documentFromCosmos = await _cosmosDbService.GetDocumentAsync(demoDocument.id);

            if (string.CompareOrdinal(demoDocument.id, documentFromCosmos.id) != 0)
            {
                throw new Exception("Document id mismatch");
            }

            _logger.Information("Document created and read successfully");

            return Ok(new { Message = "Document created and read successfully", DocumentId = documentId });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating document in CosmosDB");
            return StatusCode(500, new { Error = "Failed to create document in CosmosDB", Message = ex.Message });
        }
    }

    [HttpGet("eventgrid")]
    public async Task<IActionResult> EventGrid()
    {
        try
        {
            // Create a demo event message
            var demoEventMessage = new DemoEventMessage
            {
                Subject = "demo/events",
                Message = $"Demo Event Message {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}"
            };

            // Publish the event to EventGrid
            var eventId = await _eventGridService.PublishEventAsync(demoEventMessage);

            _logger.Information("Event published successfully");

            return Ok(new { Message = "Event published successfully", EventId = eventId });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error publishing event to EventGrid");
            return StatusCode(500, new { Error = "Failed to publish event to EventGrid", Message = ex.Message });
        }
    }
}

