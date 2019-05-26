namespace Samples.FunctionApp
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Configuration;

    public class ManualFunction
    {
        private ITestService _testService;

        public ManualFunction(ITestService serviceImpl)
        {
            _testService = serviceImpl;
        }

        [FunctionName("ManualFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            log.LogTrace("C# HTTP trigger function processed a request.");

            return await _testService.ExecuteAsync(req);
        }
    }
}
