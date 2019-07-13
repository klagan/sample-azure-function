[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(Samples.FunctionApp.MyFunctionStartup))]

namespace Samples.FunctionApp
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.AzureAppServices;

    public class MyFunctionStartup : IWebJobsStartup
    {
        [System.Obsolete]
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "kam-azure-diagnostics-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;
            }).Configure<AzureBlobLoggerOptions>(options =>
            {
                options.BlobName = "log.txt";
            });

            builder.Services.AddTransient<ITopicClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var connectionString = configuration["myServiceBusAdmin"];

                return new TopicClient(connectionString, "my-in-topic");
            });

            builder.Services.AddTransient<ITestService, TestService>();
        }
    }
}
