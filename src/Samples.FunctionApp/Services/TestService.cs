using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Samples.FunctionApp.Contracts;
using Samples.FunctionApp.Operations;

[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(MyFunctionStartup))]

namespace Samples.FunctionApp.Services
{
    public class TestService : ITestService
    {
        private ITopicClient _topicClient;

        public TestService(ITopicClient topicClient)
        {
            _topicClient = topicClient;
        }

        public async Task<IActionResult> ExecuteAsync(HttpRequest request)
        {
            string name = request.Query["name"];

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            name = name ?? data?.name;

            try
            {
                var content = $"kam-return-data ({name})";

                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content)));

                await _topicClient.SendAsync(message);
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
