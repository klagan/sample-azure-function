[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(Samples.FunctionApp.MyFunctionStartup))]

namespace Samples.FunctionApp
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public interface ITestService
    {
        Task<IActionResult> ExecuteAsync(HttpRequest request);
    }
}
