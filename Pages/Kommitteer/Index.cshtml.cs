using System;
using System.Linq;
using System.Threading.Tasks;
using CsAspnet.Models.dbcontext;
using CsAspnet.Models.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsAspnet.Pages.Kommitteer
{
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

            if (Committee == null)
                return NotFound();
            
            return Page();
        }

        public async Task<IActionResult> OnGetCommittee(int committeeId)
        {
            var committee = await _context.Committee
                .Include(c => c.Motion)
                .ThenInclude(m => m.Att)
                .FirstOrDefaultAsync(c => c.Id == committeeId);

            if (committee == null)
                return new JsonResult(false);

            return ViewTools.GetPartialView("_Committee", committee);
        }

        public async Task<IActionResult> OnGetLoadMotionAsync(int motionId)
        {
            var motion = await _context.Motion.FindAsync(motionId);
            var committee = await _context.Committee
                .Include(c => c.Motion)
                .ThenInclude(m => m.Att)
                .FirstOrDefaultAsync(c => c.Id == motion.CommitteeId);
            var motionList = committee.Motion.OrderBy(m => m.MotionNumber).ToList();
            var motionIndex = motionList.FindIndex(m => m.Id == motionId);
            
            if (motion == null)
                return new JsonResult(false);
            
            return ViewTools.GetPartialView("_Motion", Tuple.Create(motionList, motionIndex));
        }
    }
}
