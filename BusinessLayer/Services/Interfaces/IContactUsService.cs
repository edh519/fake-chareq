using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces;

public interface IContactUsService
{
    ContactUs CreateContactUsMessage(string message, string email);
    void UpdateContactUsForm(ContactUs contactUsSubmission, ContactUsViewModel contactUsViewModel, ApplicationUser currentUser);
    Task SendContactUsEmailToDevelopers(ContactUs contact, string linkUrl);
}