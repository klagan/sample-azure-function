using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Samples.FunctionApp.Models
{
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
