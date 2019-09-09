using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsAspnet.Models.dbcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsAspnet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly motionsContext _context;

        public IndexModel(motionsContext context)
        {
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            
        }
    }
}
