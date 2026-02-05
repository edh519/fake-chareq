namespace WebApp.Configuration.Subscriptions;

public class SubscriptionServiceConfig
{
    public const string SubscriptionService = "QuartzJobConfig";
    public string WebsiteUrl { get; set; }
    public string ViewWorkRequestUrl { get; set; }
}