using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samples.FunctionApp.Operations;

[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(MyFunctionStartup))]

namespace Samples.FunctionApp.Contracts
{
    public interface ITestService
    {
        Task<IActionResult> ExecuteAsync(HttpRequest request);
    }
}
