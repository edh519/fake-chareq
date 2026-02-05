using BusinessLayer.Services;
using Microsoft.Extensions.Options;
using Quartz;
using System.Threading.Tasks;
using WebApp.Configuration.Subscriptions;

public class AssignGHIssuesJob : IJob
{
  private readonly AssignGHIssuesService _assignGHIssuesService;
  private readonly AssignGHIssuesConfig _assignGHIssuesConfig;
  public AssignGHIssuesJob(AssignGHIssuesService assignGHIssuesService, IOptionsMonitor<AssignGHIssuesConfig> assignGHIssuesConfig)
  {
    _assignGHIssuesService = assignGHIssuesService;
    _assignGHIssuesConfig = assignGHIssuesConfig.CurrentValue;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    await _assignGHIssuesService.RunAssignGHIssuesService(_assignGHIssuesConfig.ViewWorkRequestUrl);
  }
}
