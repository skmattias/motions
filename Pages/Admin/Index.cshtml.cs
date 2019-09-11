using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsAspnet.Pages.Admin
{
    public class IndexModel : PageModel
    {
        [Authorize(Roles = "admin")]
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
