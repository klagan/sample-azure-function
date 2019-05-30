namespace Samples.FunctionApp
{
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using System.Text;

    public class EventMessage<T> : Message where T : class
    {
        public EventMessage(T payload)
        {
            Label = typeof(T).Name;
            Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));
        }

        public EventMessage(T payload, string correlationId)
            : this (payload)
        {
            CorrelationId = correlationId;
        }
    }
}
