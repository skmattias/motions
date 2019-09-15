using System;
using System.Threading.Tasks;
using CsAspnet.Models.dbcontext;
using CsAspnet.Models.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Microsoft.EntityFrameworkCore;

namespace CsAspnet.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly motionsContext _context;
        
        public IndexModel(motionsContext context)
        {
            _context = context;
        }
        
        public IActionResult OnGet()
        {
            return Redirect("/");
        }
        
        // 
        // Committee handlers:
        public async Task<IActionResult> OnGetEditCommittee(int committeeId)
        {
            var committee = await _context.Committee.FindAsync(committeeId);
            if (committee == null)
                return new JsonResult(false);

            return ViewTools.GetPartialView("Committee/_EditCommittee", committee);
        }
        
        public class SaveCommitteePostData
        {
            public int CommitteeId;
            public string Name;
            public string Description;
        }
        
        public async Task<IActionResult> OnPostSaveCommitteeAsync([FromBody] SaveCommitteePostData data)
        {
            try
            {
                // Check for data error.
                if (data == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Något gick fel när kommittén skulle sparas"
                    });
                
                // Check that the required data was sent.
                if (string.IsNullOrWhiteSpace(data.Name))
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Ange ett kommitténamn"
                    });

                var committee = await _context.Committee.FindAsync(data.CommitteeId);
                if (committee == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Kommittén hittades inte i databsen. Prova att ladda om sidan."
                    });

                // Edit the motion data.
                committee.CommitteeName = data.Name;
                committee.Description = data.Description;

                // Save changes and return the new name.
                await _context.SaveChangesAsync();
                return new JsonResult(new {Result = true});
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = "Något gick fel när kommittén skulle sparas. Exception: " + e.Message
                });
            }
        }
        
        //
        // Motion handlers:
        
        public async Task<IActionResult> OnGetEditMotionAsync(int motionId)
        {
            var motion = await _context.Motion
                .Include(m => m.Committee)
                .Include(m => m.Att)
                .FirstOrDefaultAsync(m => m.Id == motionId);

            if (motion == null)
                return new JsonResult(false);
            
            return ViewTools.GetPartialView("Motion/_EditMotion", motion);
        }

        public ActionResult OnGetAddMotion(int committeeId)
        {
            return ViewTools.GetPartialView("Motion/_AddMotion", committeeId);
        }

        public ActionResult OnGetCancelAddMotion()
        {
            return ViewTools.GetPartialView("Motion/_AddMotionButton");
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
            public string Argument;
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
                        Argument = data.Argument,
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
                        .Include(m => m.Att)
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
                    motion.Argument = data.Argument;
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

        public async Task<IActionResult> OnPostDeleteMotionAsync([FromBody] int motionId)
        {
            try
            {
                var motion = await _context.Motion.FindAsync(motionId);

                // Check for datbase errors.
                if (motion == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Motionen hittades inte i databsen. Prova att ladda om sidan."
                    });

                _context.Motion.Remove(motion);
                await _context.SaveChangesAsync();
                return new JsonResult(new {Result = true});
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = "Något gick fel när motionen skulle raderas. Exception: " + e.Message
                });
            }
        }
        
        //
        // Att handler
        public async Task<IActionResult> OnGetEditAttAsync(int attId)
        {
            var att = await _context.Att
                .Include(a => a.Motion)
                .ThenInclude(m => m.Committee)
                .FirstOrDefaultAsync(a => a.Id == attId);

            if (att == null)
                return new JsonResult(false);
            
            return ViewTools.GetPartialView("Att/_EditAtt", att);
        }
        
        public IActionResult OnGetAddAtt(int motionId)
        {
            return ViewTools.GetPartialView("Att/_AddAtt", motionId);
        }

        public IActionResult OnGetCancelAddAtt(int motionId)
        {
            return ViewTools.GetPartialView("Att/_AddAttButton", motionId);
        }
        
        public class SaveAttPostData
        {
            // Att id if an existing att is being edited.
            public int AttId;
            // Motion id if a new att is being added.
            public int MotionId;
            public int? Number;
            public string Text;
            public string VoteSuggestion;
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

                Att att = null;
                
                // Create a new att.
                if (data.AttId == 0)
                {
                    // Get the motion.
                    var motion = await _context.Motion.FindAsync(data.MotionId);
                    if (motion == null)
                        return new JsonResult(new
                        {
                            Result = false,
                            Message = "Motionen hittades inte i databasen. Prova att ladda on sidan."
                        });
                    
                    att = new Att
                    {
                        AttNumber = data.Number.Value,
                        AttText = data.Text,
                        Motion = motion,
                        SuggestedVote = data.VoteSuggestion
                    };

                    await _context.Att.AddAsync(att);
                }
                // Edit an existing att.
                else
                {
                    // Get the att proposition.
                    att = await _context.Att
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
                    att.AttNumber = data.Number.Value;
                    att.AttText = data.Text;
                    att.SuggestedVote = data.VoteSuggestion;
                }

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
        
        public async Task<IActionResult> OnPostDeleteAttAsync([FromBody] int attId)
        {
            try
            {
                var att = await _context.Att.FindAsync(attId);

                // Check for datbase errors.
                if (att == null)
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = "Att-satsen hittades inte i databsen. Prova att ladda om sidan."
                    });

                _context.Att.Remove(att);
                await _context.SaveChangesAsync();
                return new JsonResult(new {Result = true});
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = "Något gick fel när att-satsen skulle raderas. Exception: " + e.Message
                });
            }
        }
    }
}
