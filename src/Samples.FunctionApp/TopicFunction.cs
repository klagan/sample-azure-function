namespace Samples.FunctionApp
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using System.Transactions;

    public static class TopicFunction
    {
        [Disable]
        [FunctionName("TopicFunction")]
        public static async Task RunAsync([ServiceBusTrigger("my-in-topic", "sub1", Connection = "myServiceBusAdmin")]
        Message messageIn, ILogger log, MessageReceiver messageReceiver, string lockToken)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {messageIn}");

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
            {
                //var messageSender = new MessageSender(messageReceiver.ServiceBusConnection, "queue-out", "queue-in");

                //var outgoingMessage = new Message(Encoding.UTF8.GetBytes($"Processed message with ID {messageIn.MessageId}"));

                //log.LogInformation("Sending a message out");
                //await messageSender.SendAsync(outgoingMessage);
                //log.LogInformation("Done");

                //await messageReceiver.CompleteAsync(messageIn.SystemProperties.LockToken);


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

                scope.Complete();
            }
        }
    }
}
