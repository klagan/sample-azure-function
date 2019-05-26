[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(Samples.FunctionApp.MyFunctionStartup))]

namespace Samples.FunctionApp
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class MyFunctionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            new ServiceCollection().AddHttpClient();

            builder.Services.AddTransient<ITopicClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var connectionString = configuration["myServiceBusAdmin"];

                return new TopicClient(connectionString, "my-in-topic");
            });

            builder.Services.AddTransient<ITestService, TestService>();
        }
    }

    public interface ITestService
    {
        Task<IActionResult> ExecuteAsync(HttpRequest request);
    }

    public class TestService : ITestService
    {
        private ITopicClient _topicClient;

        public TestService (ITopicClient topicClient)
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

    public interface ITest
    {
        string TestData { get; }
    }

    public class Test : ITest
    {
        public string TestData => "My test data";
    }
}
