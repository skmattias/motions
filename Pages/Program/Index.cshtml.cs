using System;
using System.Linq;
using System.Threading.Tasks;
using CsAspnet.Models.dbcontext;
using CsAspnet.Models.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsAspnet.Pages.Program
{
    public class IndexModel : PageModel
    {
        public readonly motionsContext _context;
        public Models.dbcontext.Program Program { get; set; }

        public IndexModel(motionsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Program = await _context.Program
                .FirstOrDefaultAsync(p => p.ProgramNumber == id);

            if (Program == null)
                return NotFound();
            
            await _context.SiteLoad.AddAsync(new SiteLoad
            {
                TimeStamp = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return Page();
        }

        public async Task<IActionResult> OnGetLoadProgramAsync(int programId)
        {
            var program = await _context.Program
                .Include(p => p.Section)
                .Include(p => p.Att)
                .FirstOrDefaultAsync(p => p.Id == programId);
            
            if (program == null)
                return new JsonResult(false);

            return ViewTools.GetPartialView("_Program", program);
        }
    }
}
