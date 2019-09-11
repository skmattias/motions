using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsAspnet.Models.dbcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsAspnet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly motionsContext _context;
        
        public List<Committee> Committees { get; set; }
        
        public IndexModel(motionsContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Committees = await _context.Committee.ToListAsync();
            return Page();
        }
    }
}
