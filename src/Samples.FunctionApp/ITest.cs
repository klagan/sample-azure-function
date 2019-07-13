[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(Samples.FunctionApp.MyFunctionStartup))]

namespace Samples.FunctionApp
{
    public interface ITest
    {
        string TestData { get; }
    }
}
