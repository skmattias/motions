using System;
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

        //
        // Motion handlers.
        
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
        
        public async Task<IActionResult> OnGetEditMotionAsync(int motionId)
        {
            var motion = await _context.Motion
                .Include(m => m.Committee)
                .Include(m => m.AttProposition)
                .ThenInclude(a => a.SuggestedVote)
                .FirstOrDefaultAsync(m => m.Id == motionId);

            if (motion == null)
                return new JsonResult(false);
            
            return ViewTools.GetPartialView("_EditMotion", motion);
        }

        public ActionResult OnGetAddMotion(int committeeId)
        {
            return ViewTools.GetPartialView("_AddMotion", committeeId);
        }

        public ActionResult OnGetCancelAddMotion()
        {
            return ViewTools.GetPartialView("_AddMotionButton");
        }

        public class SaveMotionPostData
        {
            // Motion id if an existing motion is being edited.
            public int MotionId;
            // Committee id if a new motion is being added.
            public int CommitteeId;
            public int? Number;
            public string Name;
            public string Text;
        }
        public async Task<IActionResult> OnPostSaveMotionAsync([FromBody] SaveMotionPostData data)
        {
            try
            {
                // Check for data error.
                if (data == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Något gick fel när motionen skulle sparas"
                    });
                
                // Check that the required data was sent.
                if (!data.Number.HasValue)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Ange ett motionsnummer"
                    });

                // TODO create a new motion if id == 0.

                Motion motion = null;
                
                // Create a new motion.
                if (data.MotionId == 0)
                {
                    // Get the committee.
                    var committee = await _context.Committee.FindAsync(data.CommitteeId);
                    if (committee == null)
                        return new JsonResult(new
                        {
                            Result = false,
                            Message = "Kommitten hittades inte i databasen. Prova att ladda om sidan."
                        });
                    
                    motion = new Motion
                    {
                        MotionNumber = data.Number.Value,
                        MotionName = data.Name,
                        MotionText = data.Text,
                        Committee = committee
                    };

                    await _context.Motion.AddAsync(motion);
                }
                // Edit an existing motion.
                else
                {
                    // Get the motion.
                    motion = await _context.Motion
                        .Include(m => m.Committee)
                        .Include(m => m.AttProposition)
                        .ThenInclude(a => a.SuggestedVote)
                        .FirstOrDefaultAsync(m => m.Id == data.MotionId);

                    // Check for datbase errors.
                    if (motion == null)
                        return new JsonResult(new
                        {
                            Result = false,
                            Message = "Motionen hittades inte i databsen. Prova att ladda om sidan."
                        });

                    // Edit the motion data.
                    motion.MotionNumber = data.Number.Value;
                    motion.MotionName = data.Name;
                    motion.MotionText = data.Text;
                }

                // Save changes and return the new name.
                await _context.SaveChangesAsync();
                return new JsonResult(new
                {
                    Result = true,
                    Name = motion.FullName()
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = "Något gick fel när motionen skulle sparas. Exception: " + e.Message
                });
            }
        }
        
        //
        // Att proposition handlers.
        public async Task<IActionResult> OnGetEditAttAsync(int attId)
        {
            var att = await _context.AttProposition
                .Include(a => a.SuggestedVote)
                .Include(a => a.Motion)
                .ThenInclude(m => m.Committee)
                .FirstOrDefaultAsync(a => a.Id == attId);

            if (att == null)
                return new JsonResult(false);
            
            return ViewTools.GetPartialView("_EditAttProposition", att);
        }
        
        public class SaveAttPostData
        {
            public int AttId;
            public int? Number;
            public string Text;
        }
        public async Task<IActionResult> OnPostSaveAttAsync([FromBody] SaveAttPostData data)
        {
            try
            {
                // Check for data error.
                if (data == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Något gick fel när att-satsen skulle sparas"
                    });
                
                // Check that the required data was sent.
                if (!data.Number.HasValue)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Ange ett nummer för att-satsen"
                    });
                
                if (string.IsNullOrWhiteSpace(data.Text))
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Ange en text för att-satsen"
                    });

                // TODO create a new att proposition if id == 0.

                // Get the att proposition.
                var att = await _context.AttProposition
                    .Include(a => a.SuggestedVote)
                    .Include(a => a.Motion)
                    .ThenInclude(m => m.Committee)
                    .FirstOrDefaultAsync(a => a.Id == data.AttId);

                // Check for datbase errors.
                if (att == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Att-satsen hittades inte i databsen. Prova att ladda om sidan."
                    });

                // Edit the motion data.
                att.AttPropositionNumber = data.Number.Value;
                att.AttPropositionText = data.Text;

                // Save changes and return true.
                await _context.SaveChangesAsync();
                return new JsonResult(new {Result = true});
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = "Något gick fel när att-satsen skulle sparas. Exception: " + e.Message
                });
            }
        }
    }
}
