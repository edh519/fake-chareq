using BusinessLayer.ViewModels;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLayer.Services.Interfaces;
using YTULib;
using YTULib.Models;
using Newtonsoft.Json;


namespace WebApp.Controllers;

[Authorize(Roles = "Authoriser")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly YTULibServices _ytuLibServices;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUserUpdateService _userUpdateService;

    public UsersController(UserManager<ApplicationUser> userManager, YTULibServices ytuLibServices, ApplicationDbContext applicationDbContext, IUserUpdateService userUpdateService)
    {
        _userManager = userManager;
        _ytuLibServices = ytuLibServices;
        _applicationDbContext = applicationDbContext;
        _userUpdateService = userUpdateService;
    }
    public IActionResult Index(bool viewInactive = false)
    {
        UsersIndexViewModel model = new();

        List<ApplicationUser> users = _userManager.Users.ToList();

        model.SystemHasInactiveUsers = users.Any(e => e.LockoutEnabled && (e.LockoutEnd ?? DateTimeOffset.MinValue) > DateTimeOffset.Now);
        foreach (ApplicationUser identityUser in users)
        {
            if ((!viewInactive && identityUser.LockoutEnabled && (identityUser.LockoutEnd ?? DateTimeOffset.MinValue) > DateTimeOffset.Now) || identityUser.isSystemAccount)
            {
                continue;
            }
            model.Users.Add(new UserSimpleViewModel
            {
                UserId = identityUser.Id,
                Email = identityUser.Email,
                RoleAsString = string.Join(", ", _userManager.GetRolesAsync(identityUser).Result),
                IsLocked = identityUser.LockoutEnabled && (identityUser.LockoutEnd ?? DateTimeOffset.MinValue) > DateTimeOffset.Now
            });
        }
        return View(model);
    }
    public IActionResult ListNewLibraryUsers()
    {
        List<string> allCurrentSystemUsers = _applicationDbContext.Users.Select(x => x.UserName).ToList();

        List<SystemsUser> allPossibleNewOperatives = _ytuLibServices.GetAllMissingActiveUsersByUsername(allCurrentSystemUsers);

        return View(allPossibleNewOperatives);
    }

    public async Task<IActionResult> AddNewUserFromLibrary(int id)
    {
        if (id < 1)
        {
           return RedirectToAction(nameof(Index));
        }
        //its best that is breaks early - thats why this is at the top jsk
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
        environment = environment.Trim().ToUpper();
        DMSystem dmSystem = _ytuLibServices.GetDMSystemByName("ChaReq", environment);

        SystemsUser systemsUser = _ytuLibServices.GetUserById(id);

        if (systemsUser == null)
        {
            RedirectToAction(nameof(Index));
        }

        ApplicationUser chaReqUser = new() { UserName = systemsUser.Username, Email = systemsUser.Email };
        await _userManager.CreateAsync(chaReqUser);
        await _userManager.AddClaimAsync(chaReqUser, new Claim(DataAccessLayer.Models.QuasiClaim.Username, chaReqUser.UserName));

        _ytuLibServices.AddSystemsUserDMSystem(systemsUser.Id, dmSystem.Id);

        return RedirectToAction(nameof(EditUser), new { userId = chaReqUser.Id });
    }

    public async Task<IActionResult> EditUser(string userId)
    {
        ApplicationUser identityUser = await _userManager.FindByIdAsync(userId);
        if (identityUser == null)
        {
            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "User does not exist."));
            return RedirectToAction(nameof(Index));
        }

        IList<string> userRole = await _userManager.GetRolesAsync(identityUser);
        IdentityRoleEnum usersIdentityRole;

        if (userRole.Count == 0)
        {
            usersIdentityRole = IdentityRoleEnum.User; //if its a new user user just give them the lowest possible, it will ask the admin to change it on redirect anyway
        }
        else
        {
            Enum.TryParse(userRole.First(), out usersIdentityRole);
        }
        string roleAsString = usersIdentityRole.ToString();

        IList<Claim> claims = await _userManager.GetClaimsAsync(identityUser);
        string firstnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        string fullNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        UserSimpleViewModel userSimpleViewModel = new()
        {
            UserId = identityUser.Id,
            Email = identityUser.Email,
            RoleAsString = roleAsString,
            RoleId = (int)usersIdentityRole,
            Roles = RolesAsSelectList(),
            Username = identityUser.UserName,
            FirstName = firstnameClaim,
            FullName = fullNameClaim
        };
        return View(userSimpleViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(UserSimpleViewModel userSimpleViewModel)
    {
        if (!ModelState.IsValid)
        {
            userSimpleViewModel.Roles = RolesAsSelectList();
            return View(userSimpleViewModel);
        }

        ApplicationUser identityUser = await _userManager.FindByIdAsync(userSimpleViewModel.UserId);

        if (identityUser is null)
        {
            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "User does not exist."));
            return RedirectToAction(nameof(Index));
        }
        await _userUpdateService.UpdateUserDetailsAsync(
            userSimpleViewModel.Username,
            userSimpleViewModel.FirstName,
            userSimpleViewModel.FullName,
            userSimpleViewModel.Email
        );

        IdentityRoleEnum identityRole = (IdentityRoleEnum)userSimpleViewModel.RoleId;
        if (identityRole != IdentityRoleEnum.Developer)
        {
            IList<string> roles = await _userManager.GetRolesAsync(identityUser);
            roles = roles.ToList();
            await _userManager.RemoveFromRolesAsync(identityUser, roles.ToArray());
            await _userManager.AddToRoleAsync(identityUser, identityRole.ToString());
        }
        else // Developer
        {
            await _userManager.AddToRoleAsync(identityUser, identityRole.ToString());
            await _userManager.AddToRoleAsync(identityUser, IdentityRoleEnum.Authoriser.ToString());
            await _userManager.RemoveFromRoleAsync(identityUser, IdentityRoleEnum.User.ToString());
        }
        return RedirectToAction(nameof(EditUser), new { userId = identityUser.Id });
    }

    public async Task<IActionResult> ToggleUserActivation(string userId)
    {
        ApplicationUser identityUser = await _userManager.FindByIdAsync(userId);
        IList<string> userRole = await _userManager.GetRolesAsync(identityUser);
        IdentityRoleEnum usersIdentityRole;
        if (userRole.Count == 0)
        {
            usersIdentityRole = IdentityRoleEnum.User; //if its a new user user just give them the lowest possible, it will ask the admin to change it on redirect anyway
        }
        else
        {
            Enum.TryParse(userRole.First(), out usersIdentityRole);
        }
        string roleAsString = usersIdentityRole.ToString();

        UserSimpleViewModel userSimpleViewModel = new()
        {
            UserId = identityUser.Id,
            Email = identityUser.Email,
            RoleAsString = roleAsString,
            RoleId = (int)usersIdentityRole,
            Roles = RolesAsSelectList(),
            Username = identityUser.UserName,
            IsLocked = identityUser.LockoutEnabled && (identityUser.LockoutEnd ?? DateTimeOffset.MinValue) > DateTimeOffset.Now
        };
        return View(userSimpleViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleUserActivation(UserSimpleViewModel userSimpleViewModel)
    {
        if (!ModelState.IsValid)
            {
            userSimpleViewModel.Roles = RolesAsSelectList();
            return View(userSimpleViewModel);
            }
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            environment = environment.Trim().ToUpper();
            DMSystem dmSystem = _ytuLibServices.GetDMSystemByName("ChaReq", environment);

        if (dmSystem == null)
        {
            ModelState.AddModelError("", "System config error.");
            userSimpleViewModel.Roles = RolesAsSelectList();
            return View(userSimpleViewModel);
        }
            SystemsUser systemsUser = _ytuLibServices.GetUserByUsername(userSimpleViewModel.Username);

        if (systemsUser == null) 
        {
            ModelState.AddModelError("", "User not found in library.");
            userSimpleViewModel.Roles = RolesAsSelectList();
            return View(userSimpleViewModel);
        }
            ApplicationUser identityUser = await _userManager.FindByIdAsync(userSimpleViewModel.UserId);

        if (identityUser == null) 
        {
            ModelState.AddModelError("", "User not found.");
            userSimpleViewModel.Roles = RolesAsSelectList();
            return View(userSimpleViewModel);
        }
        
        if (!userSimpleViewModel.IsLocked)
        {
            //remove reference from library
            _ytuLibServices.RemoveSystemsUserDMSystem(systemsUser.Id, dmSystem.Id);
            await _userManager.SetLockoutEndDateAsync(identityUser, DateTimeOffset.MaxValue);
            return RedirectToAction(nameof(Index));
        }
            else
            {
                //remove reference from library
                _ytuLibServices.AddSystemsUserDMSystem(systemsUser.Id, dmSystem.Id);
                await _userManager.SetLockoutEndDateAsync(identityUser, DateTimeOffset.Now);
            }
        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> RolesAsSelectList()
    {
        // HACK: OrderBy hack to make the roles appear in order of permissions (User, Authoriser).
        RoleStore<IdentityRole> roleStore = new(_applicationDbContext);
        List<IdentityRole> roles = roleStore.Roles.ToList();

        List<IdentityRole> identityRoles = roles.OrderByDescending(x => x.NormalizedName == "USER").ThenBy(x => x.Id).ToList();
        List<SelectListItem> selectListRoles = new(identityRoles.Select(x => new SelectListItem { Value = x.Id, Text = x.Name }));
        return selectListRoles;
    }
}