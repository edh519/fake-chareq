using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories;

public class WorkRequestSubscriptionRepository : Repository<WorkRequestSubscription>, IWorkRequestSubscriptionRepository
{
  private readonly IWorkRequestRepository _workRequestRepository;
  public WorkRequestSubscriptionRepository(ApplicationDbContext context, IWorkRequestRepository workRequestRepository) : base(context)
  {
    _workRequestRepository = workRequestRepository;
  }

  public async Task<List<WorkRequestSubscription>> GetAllWorkRequestSubscriptions()
  {
    return await _context.WorkRequestSubscriptions.Include(e => e.ApplicationUser)
        .Include(e => e.WorkRequest)
        .ThenInclude(e => e.Status)
        .Include(e => e.WorkRequest.Trial)
        .Include(e => e.WorkRequest)
        .ThenInclude(e => e.WorkRequestEvents)
        .ToListAsync();
  }

  public async Task<List<WorkRequestSubscription>> GetUserSubscriptions(string userId)
  {
    return await _context.WorkRequestSubscriptions
        .Include(e => e.WorkRequest)
        .ThenInclude(s => s.Trial)
        .Include(e => e.WorkRequest)
        .ThenInclude(e => e.Status)
        .Where(e => e.ApplicationUser.Id == userId)
        .ToListAsync();
  }

  public async Task<List<WorkRequestSubscription>> GetWorkRequestActiveSubscribers(int workRequestId)
  {
    return await _context.WorkRequestSubscriptions
        .Include(e => e.WorkRequest)
        .Include(e => e.ApplicationUser)
        .Where(e => e.WorkRequest.WorkRequestId == workRequestId &&
                    ((e.ApplicationUser.LockoutEnabled && (e.ApplicationUser.LockoutEnd ?? DateTimeOffset.MinValue) < DateTimeOffset.Now) || !e.ApplicationUser.LockoutEnabled)) // ((lockout enabled AND lockout expired) OR lockout disabled)
    .ToListAsync();
  }

  public async Task<WorkRequestSubscription> CreateWorkRequestSubscription(string currentUserId, string targetUserId, int workRequestId)
  {
    WorkRequest? workRequest = await _context.WorkRequests.FirstOrDefaultAsync(x => x.WorkRequestId == workRequestId);
    if (workRequest is null) return null;

    WorkRequestSubscription existingSub = await _context.WorkRequestSubscriptions.FirstOrDefaultAsync(e =>
        e.WorkRequest.WorkRequestId == workRequestId && e.ApplicationUser.Id == targetUserId);
    if (existingSub is not null) return existingSub;

    ApplicationUser? currentUser = await _context.Users.FirstOrDefaultAsync(e => e.Id == currentUserId);
    ApplicationUser? targetUser = await _context.Users.FirstOrDefaultAsync(e => e.Id == targetUserId);

    WorkRequestSubscription newSub = new()
    {
      WorkRequest = await _context.WorkRequests.FirstOrDefaultAsync(e => e.WorkRequestId == workRequestId),
      ApplicationUser = targetUser
    };
    await _context.AddAsync(newSub);

    string contentEmail = targetUser.isSystemAccount ? targetUser.Email : CommonHelpers.RemoveDomainFromEmail(targetUser.Email);

    WorkRequestEvent workRequestEvent = new()
    {
      WorkRequest = workRequest,
      Content = $"{CommonHelpers.RemoveDomainFromEmail(currentUser.Email)} - subscribed - {contentEmail} at {DateTime.Now:HH:mm}",
      CreatedAt = DateTime.Now,
      CreatedBy = currentUser,
      EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Subscription)
    };

    _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);

    await _context.SaveChangesAsync();
    return newSub;
  }

  public async Task RemoveWorkRequestSubscription(string currentUserId, string targetUserId, int workRequestId)
  {
    WorkRequest? workRequest = await _context.WorkRequests.FirstOrDefaultAsync(x => x.WorkRequestId == workRequestId);
    if (workRequest is null) return;

    WorkRequestSubscription existingSub = await _context.WorkRequestSubscriptions.FirstOrDefaultAsync(e =>
        e.WorkRequest.WorkRequestId == workRequestId && e.ApplicationUser.Id == targetUserId);
    if (existingSub is null) return;

    _context.WorkRequestSubscriptions.Remove(existingSub);

    ApplicationUser? currentUser = await _context.Users.FirstOrDefaultAsync(e => e.Id == currentUserId);
    ApplicationUser? targetUser = await _context.Users.FirstOrDefaultAsync(e => e.Id == targetUserId);

    string contentEmail = targetUser.isSystemAccount ? targetUser.Email : CommonHelpers.RemoveDomainFromEmail(targetUser.Email);

    WorkRequestEvent workRequestEvent = new()
    {
      WorkRequest = workRequest,
      Content = $"{CommonHelpers.RemoveDomainFromEmail(currentUser.Email)} - unsubscribed - {contentEmail} at {DateTime.Now:HH:mm}",
      CreatedAt = DateTime.Now,
      CreatedBy = currentUser,
      EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Subscription)
    };

    _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);

    await _context.SaveChangesAsync();
  }

  public async Task<WorkRequestSubscription> GetWorkRequestSubscription(string userId, int workRequestId)
  {
    return await _context.WorkRequestSubscriptions
        .Include(e => e.WorkRequest)
        .Include(e => e.ApplicationUser)
        .Where(e => e.ApplicationUser.Id == userId && e.WorkRequest.WorkRequestId == workRequestId)
        .FirstOrDefaultAsync();
  }

  public async Task RemoveAllUserSubscriptions(string userId)
  {
    List<WorkRequestSubscription> subsToRemove = await _context.WorkRequestSubscriptions.Where(e => e.ApplicationUser.Id == userId)
        .Select(e => new WorkRequestSubscription
        {
          Id = e.Id
        }).ToListAsync();
    _context.WorkRequestSubscriptions.RemoveRange(subsToRemove);
    await _context.SaveChangesAsync();
  }

  public Task<bool> GetUserIsSubscribedToWorkRequest(string userId, int workRequestId)
  {
    return _context.WorkRequestSubscriptions.AnyAsync(s => s.ApplicationUser.Id == userId && s.WorkRequest.WorkRequestId == workRequestId);
  }
}