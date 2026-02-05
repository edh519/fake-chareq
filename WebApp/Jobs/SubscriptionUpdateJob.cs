using System.Threading.Tasks;
using BusinessLayer.Services.EmailProcessing;
using Microsoft.Extensions.Options;
using Quartz;
using WebApp.Configuration.Subscriptions;

namespace WebApp.Jobs
{
    public class SubscriptionUpdateJob : IJob
    {
        private readonly SubscriptionServiceConfig _subscriptionServiceConfig;
        private readonly WorkRequestSubscriptionUpdateService _subscriptionUpdateService;
        public SubscriptionUpdateJob(IOptionsMonitor<SubscriptionServiceConfig> subscriptionServiceConfig, WorkRequestSubscriptionUpdateService subscriptionUpdateService)
        {
            _subscriptionUpdateService = subscriptionUpdateService;
            _subscriptionServiceConfig = subscriptionServiceConfig.CurrentValue;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _subscriptionUpdateService.RunWorkRequestSubscriptionUpdate(_subscriptionServiceConfig.WebsiteUrl, _subscriptionServiceConfig.ViewWorkRequestUrl);
        }
    }
}
