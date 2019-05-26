namespace Samples.FunctionApp
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class Sender
    {
        const string ServiceBusConnectionString = "Endpoint=sb://play-bus.servicebus.windows.net/;SharedAccessKeyName=RootSender;SharedAccessKey=dDagZRtPDEE0FYhjG7uHvtKct8yr3SdQnEaUsDhdhro=";
        const string TopicName = "my-out-topic";
        const string SubscriptionName = "{Subscription Name}";
        static ITopicClient topicClient;
        //static ISubscriptionClient subscriptionClient;

        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            for (var i = 0; i < numberOfMessagesToSend; i++)
            {
                try
                {
                    // Create a new message to send to the topic
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the topic
                    await topicClient.SendAsync(message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                }
            }
        }

        public static async Task MainAsync()
        {
            const int numberOfMessages = 1;

            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            //subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

         
            await SendMessagesAsync(numberOfMessages);

            //await subscriptionClient.CloseAsync();
            await topicClient.CloseAsync();
        }
    }
}
