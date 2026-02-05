using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services;
using BusinessLayer.ViewModels.Subscriptions;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers;

public class SubscriptionsController : Controller
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IWorkRequestSubscriptionRepository _workRequestSubscriptionRepository;
  private readonly WorkRequestSubscriptionService _workRequestSubscriptionService;
  private readonly ILogger<SubscriptionsController> _logger;

  public SubscriptionsController(
      IWorkRequestSubscriptionRepository workRequestSubscriptionRepository,
      UserManager<ApplicationUser> userManager,
      ILogger<SubscriptionsController> logger, WorkRequestSubscriptionService workRequestSubscriptionService)
  {
    _workRequestSubscriptionRepository = workRequestSubscriptionRepository;
    _userManager = userManager;
    _logger = logger;
    _workRequestSubscriptionService = workRequestSubscriptionService;
  }
  [Authorize]
  public async Task<IActionResult> Index(bool showClosedWorkRequests = false)
  {
    SubscriptionsIndexViewModel model = new();
    try
    {
      ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
      model.UserId = user.Id;
      List<WorkRequestSubscription> userSubscriptions = await _workRequestSubscriptionRepository.GetUserSubscriptions(user.Id);

      // Filter out completed/abandoned requests
      List<WorkRequestStatusEnum> statusesToFilter = new()
      { WorkRequestStatusEnum.Completed, WorkRequestStatusEnum.Abandoned };

      if (!showClosedWorkRequests)
      {
        userSubscriptions = userSubscriptions.Where(e => statusesToFilter.All(s => e.WorkRequest.Status.WorkRequestStatusId != s)).ToList();
      }

      model.UserSubscriptions = userSubscriptions;
      return View(model);
    }

    catch (Exception ex)
    {
      _logger.LogError(ex, $"Error loading user subscriptions. Exception: {ex.InnerException?.Message ?? ex.Message}");
    }
    return View(model);
  }

  [ActionName("Index")]
  [HttpPost]
  public async Task<IActionResult> IndexPost(SubscriptionsIndexViewModel model, int? workRequestId)
  {
    if (model.UserId is null) return RedirectToAction(nameof(Index));
    try
    {
      if (workRequestId is not null)
      {
        await _workRequestSubscriptionService.UnsubscribeUserFromWorkRequest(model.UserId, model.UserId, workRequestId.Value);
        return RedirectToAction(nameof(Index));
      }
      await _workRequestSubscriptionRepository.RemoveAllUserSubscriptions(model.UserId);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, ex.Message);
    }
    return RedirectToAction(nameof(Index));
  }
}