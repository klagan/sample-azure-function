using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Samples.FunctionApp.Contracts;

namespace Samples.FunctionApp.Operations
{
    public class ManualFunction
    {
        private ITestService _testService;

        public ManualFunction(ITestService serviceImpl)
        {
            _testService = serviceImpl;
        }

        [Disable]
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
