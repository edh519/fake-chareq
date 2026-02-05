using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                    return LocalRedirect(returnUrl);
            }
            else
            {
                //Check if the user has been added by admin (to stop just any UoY user logging in)
                var user = _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email)).Result;
                if (user == null)
                {
                    ErrorMessage = "Error loading internal login information.";
                    return RedirectToPage("./AccessDenied");
                }
                else
                {
                    //Complete their account for the first time

                    returnUrl ??= Url.Content("~/");
                    // Get the information about the user from the external login provider

                    if (info == null)
                    {
                        ErrorMessage = "Error loading external login information during confirmation.";
                        return RedirectToPage("./AccessDenied");
                    }

                    var identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {

                        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                        {
                            await _userManager.AddClaimAsync(user, info.Principal.FindFirst(ClaimTypes.GivenName));
                        }

                        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
                        {
                            await _userManager.AddClaimAsync(user, info.Principal.FindFirst(ClaimTypes.Name));
                        }

                        TempData["FirstLogin"] = "true";

                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        //await _emailSender.SendEmailAsync(Input.Email, "SOP Manager Login",
                        //    $"Reference: You have successfully logged in to YTU's SOP Manager for the first time. If this wasn't you, please contact ytu-developers-group@york.ac.uk.");

                        return LocalRedirect(returnUrl);
                    }

                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    LoginProvider = info.LoginProvider;
                    ReturnUrl = returnUrl;
                    return Page();
                }
            }
        }
    }
}