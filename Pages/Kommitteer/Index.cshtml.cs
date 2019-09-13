using System;
using System.Threading.Tasks;
using CsAspnet.Models.dbcontext;
using CsAspnet.Models.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsAspnet.Pages.Kommitteer
{
    // TODO! lägg alla farliga handlers i admin! Testa att man inte kan getta
    // eller posta utan att vara inloggad!
    public class IndexModel : PageModel
    {
        private readonly motionsContext _context;
        public Committee Committee { get; set; }

        public IndexModel(motionsContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Committee = await _context.Committee
                .FirstOrDefaultAsync(c => c.CommitteeNumber == id);
            
            return Page();
        }

        public async Task<IActionResult> OnGetCommittee(int committeeId)
        {
            var committee = await _context.Committee
                .Include(c => c.Motion)
                .ThenInclude(m => m.AttProposition)
                .ThenInclude(a => a.SuggestedVote)
                .FirstOrDefaultAsync(c => c.Id == committeeId);

            if (committee == null)
                return new JsonResult(false);

            return ViewTools.GetPartialView("_Committee", committee);
        }

        public async Task<IActionResult> OnGetLoadMotionAsync(int motionId)
        {
            var motion = await _context.Motion
                .Include(m => m.Committee)
                .Include(m => m.AttProposition)
                .ThenInclude(a => a.SuggestedVote)
                .FirstOrDefaultAsync(m => m.Id == motionId);

            if (motion == null)
                return new JsonResult(false);
            
            return ViewTools.GetPartialView("_Motion", motion);
        }
    }
}
