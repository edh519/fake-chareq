using System.Threading.Tasks;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Microsoft.Extensions.Options;
using Quartz;
using WebApp.Configuration.Subscriptions;

namespace WebApp.Jobs;

[DisallowConcurrentExecution]
public class ProcessBulkExportJob : IJob
{
    private readonly SubscriptionServiceConfig _subscriptionServiceConfig;
    private readonly IDataExportService _dataExportService;

    public ProcessBulkExportJob(IOptions<SubscriptionServiceConfig> subscriptionServiceConfig, IDataExportService dataExportService)
    {
        _dataExportService = dataExportService;
        _subscriptionServiceConfig = subscriptionServiceConfig.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _dataExportService.ProcessBulkExportQueue(_subscriptionServiceConfig.WebsiteUrl);
    }
}