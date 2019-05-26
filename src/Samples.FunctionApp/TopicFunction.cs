namespace Samples.FunctionApp
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public static class TopicFunction
    {
        [FunctionName("TopicFunction")]
        public static async Task RunAsync([ServiceBusTrigger("my-in-topic", "sub1", Connection = "myServiceBusAdmin")]
        Message messageIn, ILogger log, MessageReceiver messageReceiver, string lockToken)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {messageIn}");

            try
            {
                if (messageIn.GetHashCode() % 3 == 0)
                {
                    await Sender.MainAsync();

                    await messageReceiver.CompleteAsync(lockToken);
                }
                else if (messageIn.GetHashCode() % 2 == 0)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (NotSupportedException e)
            {
                await messageReceiver.DeadLetterAsync(lockToken, deadLetterReason: "Bad request", deadLetterErrorDescription: e.ToString());
            }
            catch (Exception)
            {
                await messageReceiver.AbandonAsync(lockToken);
            }
        }
    }
}
