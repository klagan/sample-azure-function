namespace Samples.FunctionApp
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Microsoft.Azure.ServiceBus.Core;
    using System;
    using System.Threading;
    using Microsoft.Azure.ServiceBus;

    public static class Function1
    {
        [Disable]
        [FunctionName("AutomaticFunction")]
        [return: ServiceBus("my-out-topic", Connection = "ConnectionStrings:myServiceBusWrite")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, MessageReceiver messageReceiver)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            messageReceiver.RegisterMessageHandler(async (message, token) =>
                    {
                        Console.WriteLine($"Received message: Id:{message.MessageId} Body:{message.Body}");

                        // Complete the message so that it is not received again.
                        // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode.
                        //await queueClient.CompleteAsync(message.LockToken);
                        await Task.CompletedTask;
                    },
                    new MessageHandlerOptions(args =>
                    {
                        Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
                        return Task.CompletedTask;
                    })
                        { AutoComplete = false, MaxConcurrentCalls = 32 }
                    );


            return name;
        }
    }
}
