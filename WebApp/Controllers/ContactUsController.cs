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
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Controllers;

[Authorize(Roles = "Developer")]
public class ContactUsController : Controller
{
    private readonly IContactUsRepository _contactUsRepository;
    private readonly IContactUsService _contactUsService;
    private readonly ILogger<ContactUsController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    public ContactUsController(IContactUsRepository contactUsRepository, IContactUsService contactUsService, ILogger<ContactUsController> logger, UserManager<ApplicationUser> userManager)
    {
        _contactUsRepository = contactUsRepository;
        _contactUsService = contactUsService;
        _logger = logger;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        IEnumerable<ContactUs> contacts = _contactUsRepository.Get();

        IEnumerable<ContactUsViewModel> contactViewModels = contacts.Select(contactUs => new ContactUsViewModel
        {
            Id = contactUs.ContactUsId,
            Email = contactUs.Email,
            Message = contactUs.Message,
            Submitted = contactUs.Submitted,
            Notes = contactUs.Notes,
            Actioned = contactUs.Actioned,
            ActionedBy = contactUs.ActionedBy,
            ActionedAt = contactUs.ActionedAt
        }).AsEnumerable();

        return View(contactViewModels);
    }

    public IActionResult Details(int? id)
    {
        if (id is null) return RedirectToAction("Index");

        ContactUs? contactUs = _contactUsRepository.GetByID(id);

        if (contactUs is null) return RedirectToAction("Index");

        ContactUsViewModel model = new ContactUsViewModel
        {
            Id = contactUs.ContactUsId,
            Email = contactUs.Email,
            Message = contactUs.Message,
            Submitted = contactUs.Submitted,
            Notes = contactUs.Notes,
            Actioned = contactUs.Actioned,
            ActionedBy = contactUs.ActionedBy,
            ActionedAt = contactUs.ActionedAt
        };

        return View(model);
    }

    [ActionName("Details"), HttpPost]
    public async Task<IActionResult> DetailsPostAsync(ContactUsViewModel? model)
    {
        if (!ModelState.IsValid) return View(model);

        ContactUs? contactUs = _contactUsRepository.GetByID(model.Id);

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);

        if (contactUs is null)
        {
            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Error updating the form, please try again."));
            return View(model);
        }

        try
        {
            _contactUsService.UpdateContactUsForm(contactUs, model, applicationUser);
            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Form updated successfully."));

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Error updating the form, please try again."));
            return View(model);
        }
    }
}