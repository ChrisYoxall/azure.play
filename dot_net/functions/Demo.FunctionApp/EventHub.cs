using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demo.FunctionApp;

public static class EventHub
{
    [FunctionName("EventHub")]
    public static void RunAsync([EventHubTrigger("sa-diagnostics", Connection = "EventHubConnectionMI")] EventData[] events, ILogger log)
    {
        foreach (var eventData in events)
        {
            var records = eventData.Data.ToObjectFromJson<Records>();
            log.LogInformation($"URI: {records.RecordList[0].File}");
        }
    }
}

public class Records
{
    [JsonPropertyName("records")]
    public List<Record> RecordList { get; set; }
}

public class Record
{
    [JsonPropertyName("operationName")]
    public string OperationName { get; set; }
    
    [JsonPropertyName("uri")]
    public Uri File { get; set; }
}