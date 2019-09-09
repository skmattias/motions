using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class SuggestedVote
    {
        public int Id { get; set; }
        public int AttPropositionId { get; set; }
        public string SuggestedVote1 { get; set; }
        public string Actor { get; set; }

        public AttProposition AttProposition { get; set; }
    }
}
