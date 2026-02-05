using System.Threading.Tasks;
using BusinessLayer.Services.EmailProcessing;
using Microsoft.Extensions.Options;
using Quartz;
using WebApp.Configuration.Subscriptions;

public class StaleRequestsJob : IJob
{
    private readonly StaleRequestsConfig _staleRequestsConfig;
    private readonly StaleRequestsService _staleRequestsService;
    public StaleRequestsJob(IOptionsMonitor<StaleRequestsConfig> staleRequestsConfig, StaleRequestsService staleRequestsService)
    {
        _staleRequestsService = staleRequestsService;
        _staleRequestsConfig = staleRequestsConfig.CurrentValue;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _staleRequestsService.RunStaleRequestsService(_staleRequestsConfig.WebsiteUrl, _staleRequestsConfig.ViewWorkRequestUrl);
    }
}
