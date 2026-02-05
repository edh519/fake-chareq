using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLayer.Services;

public class UserUpdateService : IUserUpdateService
{
    private readonly ApplicationDbContext _context;
    private readonly ITrialRepository _trialRepository;

    public UserUpdateService(ApplicationDbContext context, ITrialRepository trialRepository)
    {
        _context = context;
        _trialRepository = trialRepository;
    }

    public async Task UpdateUserDetailsAsync(string username, string firstName, string fullName, string email)
    {
        string normalisedUsername = username.ToUpper();
        ApplicationUser user = await _context.Users.SingleOrDefaultAsync(u => u.NormalizedUserName == normalisedUsername);
        if (user == null)
        {
            return;
        }

        string oldEmail = user.Email;

        // Update email in related tables
        await UpdateEmailInRelatedTables(oldEmail, email);

        // Update AspNetUsers table
        user.Email = email;
        user.NormalizedEmail = email.ToUpper();
        _context.Users.Update(user);

        // Update AspNetUserClaims table
        await UpdateUserClaims(user.Id, firstName, fullName);

        await _context.SaveChangesAsync();
    }

    private async Task UpdateEmailInRelatedTables(string oldEmail, string newEmail)
    {
        await _context.FileUploads
            .Where(f => f.UploadedBy == oldEmail && f.UploadedBy != newEmail)
            .ForEachAsync(f => f.UploadedBy = newEmail);

        await _context.FinalAuthorisations
            .Where(f => f.CompletedBy == oldEmail && f.CompletedBy != newEmail)
            .ForEachAsync(f => f.CompletedBy = newEmail);

        await _context.InitialAuthorisations
            .Where(i => i.DecisionBy == oldEmail && i.DecisionBy != newEmail)
            .ForEachAsync(i => i.DecisionBy = newEmail);

        await _context.Notifications
            .Where(n => n.Recipient == oldEmail && n.Recipient != newEmail)
            .ForEachAsync(n => n.Recipient = newEmail);

        await _context.Notifications
            .Where(n => n.CreatedBy == oldEmail && n.CreatedBy != newEmail)
            .ForEachAsync(n => n.CreatedBy = newEmail);

        await _context.WorkRequests
            .Where(w => w.CreatedBy == oldEmail && w.CreatedBy != newEmail)
            .ForEachAsync(w => w.CreatedBy = newEmail);

        await _context.WorkRequests
            .Where(w => w.LastEditedBy == oldEmail && w.LastEditedBy != newEmail)
            .ForEachAsync(w => w.LastEditedBy = newEmail);
    }

    private async Task UpdateUserClaims(string userId, string firstName, string fullName)
    {
        IdentityUserClaim<string> givenNameClaim = await _context.UserClaims
            .SingleOrDefaultAsync(c => c.UserId == userId && c.ClaimType == ClaimTypes.GivenName);
        if (givenNameClaim != null && givenNameClaim.ClaimValue != firstName)
        {
            givenNameClaim.ClaimValue = firstName;
            _context.UserClaims.Update(givenNameClaim);
        }

        IdentityUserClaim<string> nameClaim = await _context.UserClaims
            .SingleOrDefaultAsync(c => c.UserId == userId && c.ClaimType == ClaimTypes.Name);
        if (nameClaim != null && nameClaim.ClaimValue != fullName)
        {
            nameClaim.ClaimValue = fullName;
            _context.UserClaims.Update(nameClaim);
        }
    }
    /// <summary>
    /// There are essentially 8 scenarios here:
    /// 1. Trial email is renamed, new email doesn't yet exist, old email only exists in this trial - current AppUser can be safely renamed
    /// 2. Trial email is renamed, new email exists in other trials, old email only exists in this trial - current AppUser can be deleted, new AppUser can be used
    /// 3. Trial email is renamed, new email doesn't yet exist, old email exists in other trials - current AppUser can be safely removed from trial, new AppUser can be created
    /// 4. Trial email is renamed, new email exists in other trials, old email exists in other trials - current AppUser can be safely removed from trial, new AppUser can be used
    /// 5. Trial email is removed and only exists in this trial - current AppUser can be deleted
    /// 6. Trial email is removed and exists in multiple trials - current AppUser can be removed from trial
    /// 7. Trial email is added and doesn't yet exist - new AppUser can be created
    /// 8. Trial email is added and already exists in multiple trials - new AppUser can be retrieved and used
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="newEmail"></param>
    public ApplicationUser? UpdateTrialEmails(ApplicationUser? currentUser, string newEmail)
    {
        bool newEmailIsEmpty = string.IsNullOrWhiteSpace(newEmail);

        ApplicationUser? existingUserForNewEmail = null;
        if (!newEmailIsEmpty)
        {
            existingUserForNewEmail = _context.Users.FirstOrDefault(u => u.Email.ToLower() == newEmail.ToLower());
        }

        // Scenarios for adding a trial email
        if (currentUser == null)
        {
            // Scenario 8
            if (existingUserForNewEmail != null)
            {
                return existingUserForNewEmail;
            }

            // Scenario 7
            if (!newEmailIsEmpty)
            {
                ApplicationUser newUser = CreateNewTrialEmail(newEmail);
                _context.Users.Add(newUser);
                return newUser;
            }
            // Trial email not changed
            return null;
        }
        List<Trial> trialsUsingCurrent = GetAllTrialsWithATrialEmail(currentUser);
        bool currentUserIsShared = trialsUsingCurrent.Count > 1;

        // Scenarios for removing a trial email
        if (newEmailIsEmpty)
        {
            // Scenario 5
            if (!currentUserIsShared)
            {
                _context.Users.Remove(currentUser);
            }
            // Scenario 6
            return null;
        }
        // Scenarios for renaming a trial email

        if (existingUserForNewEmail != null)
        {
            // Scenario 2
            if (!currentUserIsShared)
            {
                _context.Users.Remove(currentUser);
            }
            // Scenario 4
            return existingUserForNewEmail;
        }
        // Scenario 1
        if (!currentUserIsShared)
        {
            currentUser.Email = newEmail;
            currentUser.NormalizedEmail = newEmail.ToUpperInvariant();
            currentUser.UserName = CommonHelpers.RemoveDomainFromEmail(newEmail);
            currentUser.NormalizedUserName = currentUser.UserName.ToUpperInvariant();

            _context.Users.Update(currentUser);
            return currentUser;
        }
        // Scenario 3
        ApplicationUser createdUser = CreateNewTrialEmail(newEmail);
        _context.Users.Add(createdUser);
        return createdUser;
    }



    public ApplicationUser CreateNewTrialEmail(string trialEmail)
    {
        if (trialEmail == null) return null;

        ApplicationUser user = new ApplicationUser
        {
            UserName = CommonHelpers.RemoveDomainFromEmail(trialEmail),
            NormalizedUserName = CommonHelpers.RemoveDomainFromEmail(trialEmail.ToUpperInvariant()),
            Email = trialEmail,
            NormalizedEmail = trialEmail.ToUpperInvariant(),
            isSystemAccount = true
        };

        return user;
    }

    private List<Trial> GetAllTrialsWithATrialEmail(ApplicationUser user)
    {
        if (user == null) return null;

        List<Trial> allTrials = _trialRepository.Get().ToList();

        List<Trial> trialEmailTrials = allTrials.Where(x => x.TrialEmail == user).ToList();

        return trialEmailTrials;
    }

}