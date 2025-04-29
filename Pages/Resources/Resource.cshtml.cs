using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Resources
{
    [Authorize(Roles="admin,resident")]

    public class ResourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
