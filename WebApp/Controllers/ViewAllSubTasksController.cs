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
    public class ViewAllSubTasksController : Controller
    {
        private readonly ILogger<ViewAllSubTasksController> _logger;
        private readonly IViewAllSubTasksService _viewAllSubTasksService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILabelRepository _labelRepository;
        private readonly ISubTaskStatusRepository _subTaskStatusRepository;
        private readonly ITrialRepository _trialRepository;
        private readonly ISubTaskRepository _subTaskRepository;


        public ViewAllSubTasksController(ILogger<ViewAllSubTasksController> logger,
            IViewAllSubTasksService viewAllSubTasksService, UserManager<ApplicationUser> userManager,
            ILabelRepository labelRepository, ISubTaskStatusRepository subTaskStatusRepository, ITrialRepository trialRepository, ISubTaskRepository subTaskRepository)
        {
            _logger = logger;
            _viewAllSubTasksService = viewAllSubTasksService;
            _userManager = userManager;
            _labelRepository = labelRepository;
            _subTaskStatusRepository = subTaskStatusRepository;
            _trialRepository = trialRepository;
            _subTaskRepository = subTaskRepository;
        }
        [HttpGet]
        public IActionResult SubTasks(bool loadAll = false)
        {
            IEnumerable<SubTask> subTasks = [];
            if (!loadAll)
            {
                DateTime within1Year = DateTime.Now.Date.AddYears(-1);
                subTasks = _viewAllSubTasksService.GetAllSubTasks(within1Year);
            } 
            else
            {
                subTasks = _viewAllSubTasksService.GetAllSubTasks();
            }

            string jsonString = JsonConvert.SerializeObject(new ViewAllSubTasksViewModel(subTasks, _subTaskRepository));
            return Ok(jsonString);
        }
        public IActionResult Index()
        {
            ViewAllSubTasksIndexViewModel viewAll = new ViewAllSubTasksIndexViewModel
            {
                Statuses = _subTaskStatusRepository.GetAllSubTaskStatuses(),
                Trials = _trialRepository.Get()
                                        .Distinct()
                                        .OrderBy(t => t.TrialName)
                                        .ToList(),
                CurrentUser = _userManager.GetUserAsync(User).Result.Email.Split('@')[0]
            };

            return View(viewAll);
        }
    }
}
