using Microsoft.Azure.Cosmos;
using Demo.Models;
using Demo.Providers;
using ILogger = Serilog.ILogger;

 namespace Demo.Services;
 
 public interface ICosmosDbService
 {
     Task<string> CreateDocumentAsync(DemoDocument document);
     Task<DemoDocument> GetDocumentAsync(string id);
 }

 public class CosmosDbService : ICosmosDbService
 {
     private readonly Container _itemsContainer;
     private readonly ILogger _logger;

     public CosmosDbService(ICosmosClientProvider clientProvider, ILogger logger)
     {
         _itemsContainer = clientProvider.ItemsContainer;
         _logger = logger.ForContext<CosmosDbService>();
     }
     

     public async Task<string> CreateDocumentAsync(DemoDocument document)
     {
         try
         {
             ItemResponse<DemoDocument> response = await _itemsContainer.CreateItemAsync(document, new PartitionKey(document.id));
             _logger.Information("Document created with id {DocumentId}. Request charge: {RequestCharge} RUs", document.id, response.RequestCharge);
             return document.id;
         }
         catch (CosmosException ex)
         {
             _logger.Error(ex, "Error creating document in CosmosDB: {ErrorMessage}", ex.Message);
             throw;
         }
     }

     public async Task<DemoDocument> GetDocumentAsync(string id)
     {
         try
         {
             var response = await _itemsContainer.ReadItemAsync<DemoDocument>(id, new PartitionKey(id));
             _logger.Information("Document retrieved with id {DocumentId}. Request charge: {RequestCharge} RUs", id, response.RequestCharge);
             return response.Resource;
         }
         catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
         {
             _logger.Error("Document with id {DocumentId} not found", id);
             throw;
         }
         catch (CosmosException ex)
         {
             _logger.Error(ex, "Error retrieving document from CosmosDB: {ErrorMessage}", ex.Message);
             throw;
         }

     }
 }
