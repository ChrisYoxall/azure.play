using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Demo.FunctionApp;

public static class AzureFile
{
    [FunctionName("AzureFile")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger log)
    {
        const string connectionString = "BlobEndpoint=https://christestsasec.blob.core.windows.net/;QueueEndpoint=https://christestsasec.queue.core.windows.net/;FileEndpoint=https://christestsasec.file.core.windows.net/;TableEndpoint=https://christestsasec.table.core.windows.net/;SharedAccessSignature=sv=2021-12-02&ss=f&srt=sco&sp=rwdlc&se=2023-04-24T04:03:55Z&st=2023-04-09T20:03:55Z&spr=https&sig=AsIAkDcybgc1OfRsB8P17i4KJMnTBLjzihkRJDFw2JM%3D";
        const string shareName = "myshare";
        const string fileName = "test.csv";

        // Create a ShareClient instance
        var share = new ShareClient(connectionString, shareName);

        // Connect to file share
        if (!await share.ExistsAsync())
        {
            log.Log(LogLevel.Information, $"Can't connect to {share.Name}.");
            return new NotFoundResult();
        }

        // Create file client for desired file
        var directory = share.GetDirectoryClient("/");
        var file = directory.GetFileClient(fileName);

        // Read file and log each line.
        await using (var stream = await file.OpenReadAsync().ConfigureAwait(false))
        using (var reader = new StreamReader(stream)) {
            log.Log(LogLevel.Information, $"Reading file: {fileName}.");
            while (!reader.EndOfStream)
                log.Log(LogLevel.Information, $"Current line: {await reader.ReadLineAsync().ConfigureAwait(false)}");
        }
        
        log.Log(LogLevel.Information, $"Finished reading file: {fileName}.");
        return new OkResult();
    }
}