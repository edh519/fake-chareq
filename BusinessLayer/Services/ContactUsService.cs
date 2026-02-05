using BusinessLayer.Helpers;
using BusinessLayer.Repositories;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.EmailProcessing;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTU.EmailService;

namespace BusinessLayer.Services;

public class ContactUsService : IContactUsService
{
    private readonly ILogger<ContactUsService> _logger;
    private readonly EmailHandlerService _emailHandlerService;
    private readonly IContactUsRepository _contactUsRepository;
    public ContactUsService(ILogger<ContactUsService> logger, EmailHandlerService emailHandlerService, IContactUsRepository contactUsRepository)
    {
        _logger = logger;
        _emailHandlerService = emailHandlerService;
        _contactUsRepository = contactUsRepository;
    }

    public ContactUs CreateContactUsMessage(string message, string email)
    {
        ContactUs contact = new ContactUs()
        {
            Message = message,
            Email = email,
            Submitted = DateTime.Now
        };

        _contactUsRepository.AddContactUsSubmission(contact);

        return contact;
    }

    public void UpdateContactUsForm(ContactUs contactUsSubmission, ContactUsViewModel contactUsViewModel, ApplicationUser currentUser)
    {
        contactUsSubmission.ActionedBy = currentUser.Email;
        contactUsSubmission.ActionedAt = contactUsViewModel.ActionedAt;
        contactUsSubmission.Actioned = contactUsViewModel.Actioned;
        contactUsSubmission.Notes = contactUsViewModel.Notes;
        contactUsSubmission.UpdatedBy = currentUser.Email;

        _contactUsRepository.Update(contactUsSubmission);

        _contactUsRepository.Save();
    }

    public async Task SendContactUsEmailToDevelopers(ContactUs contact, string linkUrl)
    {
        //save the image to dbo.ContactUs but do not send the image in the email
        ContactUs emailContact = new ContactUs
        {
            Message = HtmlHelper.RemoveImageElementsFromString(contact.Message, false),
            Email = contact.Email,
            Submitted = DateTime.Now
        };

        RazorToHtmlParser razorParser = new();

        ContactUsEmailViewModel contactUsEmailViewModel = new(emailContact);
        contactUsEmailViewModel.LinkToSystem = linkUrl;

        try
        {
            string notificationEmail = await razorParser.RenderHtmlStringAsync("ContactUsEmail", contactUsEmailViewModel);

            Email email = new()
            {
                Subject = $"New Contact Us message from {CommonHelpers.SalutationFromEmail(contact.Email)}",
                Body = notificationEmail,
                CustomFooter = "",
                RemoveDevEmailBody = false,
                ToAddresses = "ytu-developers-group+chareq@york.ac.uk"
            };

            await _emailHandlerService.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ContactUs Email failed to send.");
        }
    }
}
