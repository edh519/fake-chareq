using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories.Interfaces;

public interface IWorkRequestSubscriptionRepository
{
  public Task<List<WorkRequestSubscription>> GetAllWorkRequestSubscriptions();
  public Task<List<WorkRequestSubscription>> GetUserSubscriptions(string userId);
  public Task<WorkRequestSubscription> CreateWorkRequestSubscription(string currentUserId, string targetUserId, int workRequestId);
  public Task<bool> GetUserIsSubscribedToWorkRequest(string userId, int workRequestId);
  public Task RemoveWorkRequestSubscription(string currentUserId, string targetUserId, int workRequestId);
  public Task<WorkRequestSubscription> GetWorkRequestSubscription(string userId, int workRequestId);
  public Task RemoveAllUserSubscriptions(string userId);
  public Task<List<WorkRequestSubscription>> GetWorkRequestActiveSubscribers(int workRequestId);
}