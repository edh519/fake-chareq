using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces;

public interface IUserUpdateService
{
    Task UpdateUserDetailsAsync(string username, string firstName, string fullName, string email);
    ApplicationUser? UpdateTrialEmails(ApplicationUser? currentUser, string newEmail);
    ApplicationUser CreateNewTrialEmail(string trialEmail);
}