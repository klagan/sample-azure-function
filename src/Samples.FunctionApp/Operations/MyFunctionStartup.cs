[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(Samples.FunctionApp.Operations.MyFunctionStartup))]

namespace Samples.FunctionApp.Operations
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.AzureAppServices;
    using Samples.FunctionApp.Contracts;
    using Samples.FunctionApp.Services;

    public class MyFunctionStartup : IWebJobsStartup
    {
        [System.Obsolete]
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services
                //.AddLogging(logBuilder =>
                //{
                //    logBuilder.AddConsole();
                //    // logBuilder.AddAzureWebAppDiagnostics();
                //})
                .Configure<AzureFileLoggerOptions>(options =>
                {
                    options.FileName = "myfiles/kam-azure-diagnostics-";
                    options.FileSizeLimit = 50 * 1024;
                    options.RetainedFileCountLimit = 5;
                })
                .Configure<AzureBlobLoggerOptions>(options =>
                {
                    options.BlobName = "myblob/log.txt";
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
