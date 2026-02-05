using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Impossible")]
    public class LockoutModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
