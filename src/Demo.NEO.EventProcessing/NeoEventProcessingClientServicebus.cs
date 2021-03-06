using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingClientServicebus
    {
        [FunctionName(nameof(NeoEventProcessingClientServicebus))]
        public async Task Run(
            [ServiceBusTrigger("neo-events", "%SubscriptionName%", Connection = "NEOEventsTopic")]DetectedNeoEvent detectedNeoEvent, 
            [DurableClient]IDurableClient durableClient,
            ILogger log)
        {
            //var detectedNeoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(message);
            var instanceId = await durableClient.StartNewAsync(nameof(NeoEventProcessingOrchestrator), 
                detectedNeoEvent);

            log.LogInformation($"Servicebus started orchestration with ID {instanceId}.");
        }        
    }
}
