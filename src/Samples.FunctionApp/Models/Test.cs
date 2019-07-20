using Samples.FunctionApp.Contracts;
using Samples.FunctionApp.Operations;

[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(MyFunctionStartup))]

namespace Samples.FunctionApp.Models
{
    public class Test : ITest
    {
        public string TestData => "My test data";
    }
}
