[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(Samples.FunctionApp.MyFunctionStartup))]

namespace Samples.FunctionApp
{
    public class Test : ITest
    {
        public string TestData => "My test data";
    }
}
