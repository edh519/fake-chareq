using BusinessLayer.Repositories;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Controllers
{
    [Authorize(Roles = "User, Authoriser")]
    public class ViewAllWorkRequestsController : Controller
    {
        private readonly ILogger<ViewAllWorkRequestsController> _logger;
        private readonly IViewAllWorkRequestsService _viewAllWorkRequestService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILabelRepository _labelRepository;
        private readonly IWorkRequestStatusRepository _workRequestStatusRepository;
        private readonly ITrialRepository _trialRepository;


        public ViewAllWorkRequestsController(ILogger<ViewAllWorkRequestsController> logger, 
            IViewAllWorkRequestsService viewAllWorkRequestService, UserManager<ApplicationUser> userManager, 
            ILabelRepository labelRepository, IWorkRequestStatusRepository workRequestStatusRepository, ITrialRepository trialRepository)
        {
            _logger = logger;
            _viewAllWorkRequestService = viewAllWorkRequestService;
            _userManager = userManager;
            _labelRepository = labelRepository;
            _workRequestStatusRepository = workRequestStatusRepository;
            _trialRepository = trialRepository;
        }
        [HttpGet]
        public IActionResult WorkRequests(bool loadAll = false)
        {
            IEnumerable<WorkRequest> workRequests = [];
            if (loadAll == false)
            {
                DateTime within1Year = DateTime.Now.Date.AddYears(-1);
                workRequests = _viewAllWorkRequestService.GetAllWorkRequests(within1Year);
            }
            else
            {
                workRequests = _viewAllWorkRequestService.GetAllWorkRequests();
            }

            string jsonString = JsonConvert.SerializeObject(new ViewAllWorkRequestsViewModel(workRequests));
            return Ok(jsonString);
        }

        [HttpGet]
        public IActionResult ParticipatingWorkRequests(bool? isFinalised = null)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            IEnumerable<WorkRequest> workRequests = _viewAllWorkRequestService.GetAllParticipatingWorkRequests(user, isFinalised);

            string jsonString = JsonConvert.SerializeObject(new ViewAllWorkRequestsViewModel(workRequests));
            return Ok(jsonString);
        }
        public IActionResult Index()
        {
            ViewAllWorkRequestsIndexViewModel viewAll = new ViewAllWorkRequestsIndexViewModel
            {
                Statuses = _workRequestStatusRepository.GetAllWorkRequestStatuses(),
                Trials = _trialRepository.Get()
                                        .Select(t => new Trial
                                        {
                                            TrialId = t.TrialId,
                                            TrialName = t.IsActive ? t.TrialName : $"{t.TrialName} (archived)",
                                            IsActive = t.IsActive
                                        })
                                        .DistinctBy(t => t.TrialName)
                                        .OrderByDescending(t => t.IsActive)
                                        .ThenBy(t => t.TrialName)
                                        .ToList(),
                Labels = _labelRepository.GetLabels()
                                        .Distinct()
                                        .OrderBy(l => l.LabelShort)
                                        .ToList(),
                CurrentUser = _userManager.GetUserAsync(User).Result.Email.Split('@')[0]
            };

            return View(viewAll);
        }
    }
}
