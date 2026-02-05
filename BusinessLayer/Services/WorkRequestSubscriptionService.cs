using BusinessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
  public class WorkRequestSubscriptionService
  {
    private readonly IWorkRequestSubscriptionRepository _workRequestSubscriptionRepository;
    private readonly ILogger<WorkRequestSubscriptionService> _logger;

    public WorkRequestSubscriptionService(IWorkRequestSubscriptionRepository workRequestSubscriptionRepository, ILogger<WorkRequestSubscriptionService> logger)
    {
      _workRequestSubscriptionRepository = workRequestSubscriptionRepository;
      _logger = logger;
    }
    public async Task UnsubscribeUserFromWorkRequest(string currentUserId, string targetUserId, int workRequestId, bool unsubscribeAll = false)
    {
      if (currentUserId == null || targetUserId == null) throw new ArgumentNullException();
      try
      {
        if (unsubscribeAll)
        {
          await _workRequestSubscriptionRepository.RemoveAllUserSubscriptions(targetUserId);
          return;
        }
        if (await _workRequestSubscriptionRepository.GetWorkRequestSubscription(targetUserId, workRequestId) is { } subscription)
        {
          await _workRequestSubscriptionRepository.RemoveWorkRequestSubscription(currentUserId, subscription.ApplicationUser.Id, subscription.WorkRequest.WorkRequestId);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to unsubscribe user id {targetUserId} from work request Id {workRequestId}. Exception: {ex.InnerException?.Message ?? ex.Message}");
      }
    }
  }
}
