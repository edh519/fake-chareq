using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
  [Authorize(Roles = "User, Authoriser")]
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IContactUsService _contactUsService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IDashboardRepository dashboardRepository, IContactUsService contactUsService, UserManager<ApplicationUser> userManager)
    {
      _logger = logger;
      _dashboardRepository = dashboardRepository;
      _contactUsService = contactUsService;
      _userManager = userManager;
    }

    private string GetCurrentUserEmail()
    {
      return _userManager.GetEmailAsync(_userManager.GetUserAsync(User).Result).Result;
    }

    public IActionResult Index(string dateInput = "12m")
    {
      List<string> allowedDateInputs = new List<string> { "1w", "4w", "3m", "6m", "12m" };

      if (!allowedDateInputs.Contains(dateInput))
      {
        return RedirectToAction(nameof(Index), new { dateInput = "12m" }); //default is 12m
      }

      ViewBag.DateInput = dateInput;

      HomeViewModel viewModel = new HomeViewModel
      {
        TotalWRsPerTrial = _dashboardRepository.GetWRsByTrial(dateInput),
        AllWRsByStatus = _dashboardRepository.GetAllWRsByStatus(dateInput),
        AverageDecisionsAndCompletedTime = _dashboardRepository.GetAverageDecisionAndCompletionTime(dateInput),
        DecisionsAndCompletedTime = _dashboardRepository.GetDecisionAndCompletionTime(dateInput),
        WREventsGroupAssignmentSplit = _dashboardRepository.GetAssignmentGroupCounts(dateInput),
        AgeOfIncompleteTasks = _dashboardRepository.GetCurrentAgeOfIncompleteTasks(dateInput)
      };
      return View(viewModel);
    }

    [AllowAnonymous]
    public IActionResult Support()
    {
      return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
      return View();
    }

    [AllowAnonymous]
    public IActionResult Accessibility()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult ContactUs()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ContactUsAsync(ContactUsViewModel contactUsViewModel)
    {
      if (!ModelState.IsValid)
      {
        return View(contactUsViewModel);
      }
      string userEmail = GetCurrentUserEmail();
      //Email cannot be identified from User - log error
      if (string.IsNullOrWhiteSpace(userEmail))
      {
        return RedirectToAction(nameof(ContactUs));
      }

      ContactUs contact = _contactUsService.CreateContactUsMessage(contactUsViewModel.Message, userEmail);
      await _contactUsService.SendContactUsEmailToDevelopers(contact, GetContactUsLink(contact.ContactUsId));
      TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Contact Us message has been submitted successfully."));

      return RedirectToAction(nameof(ContactUs));
    }

    private string GetContactUsLink(int contactUsId)
    {
      string systemLinkUrl = Url.Action(nameof(ContactUsController.Details),
                                            ControllerHelpers.GetControllerName<ContactUsController>(),
                                            new { id = contactUsId },
                                            protocol: Request.Scheme, host: Request.Host.Value);
      return systemLinkUrl;
    }
  }
}
