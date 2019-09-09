using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class AttProposition
    {
        public AttProposition()
        {
            SuggestedVote = new HashSet<SuggestedVote>();
        }

        public int Id { get; set; }
        public int AttPropositionNumber { get; set; }
        public string AttPropositionText { get; set; }
        public int MotionId { get; set; }

        public Motion Motion { get; set; }
        public ICollection<SuggestedVote> SuggestedVote { get; set; }
    }
}
