namespace Samples.FunctionApp
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Microsoft.Azure.ServiceBus;
    using System.Text;
    using Microsoft.Extensions.Configuration;

    public static class ManualFunction
    {
        [FunctionName("ManualFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log, 
            ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            try
            {
                var config = new ConfigurationBuilder()
                             .SetBasePath(context.FunctionAppDirectory)
                             .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
                             .AddEnvironmentVariables()
                             .Build();

                var connectionString = config["ConnectionStrings:myServiceBusWrite"];

                var content = "kam-return";
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content)));
                
                var topicClient = new TopicClient(connectionString, "my-in-topic");
                await topicClient.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
