namespace WebApp.Configuration.Subscriptions;

public class StaleRequestsConfig
{
    public const string StaleRequests = "QuartzJobConfig";
    public string WebsiteUrl { get; set; }
    public string ViewWorkRequestUrl { get; set; }
}
