using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Att
    {
        public Att()
        {
            SuggestedVote = new HashSet<SuggestedVote>();
        }

        public int Id { get; set; }
        public int AttNumber { get; set; }
        public string AttText { get; set; }
        public int MotionId { get; set; }

        public Motion Motion { get; set; }
        public ICollection<SuggestedVote> SuggestedVote { get; set; }
    }
}
