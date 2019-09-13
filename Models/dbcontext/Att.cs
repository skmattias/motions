using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Att
    {
        public int Id { get; set; }
        public int AttNumber { get; set; }
        public string AttText { get; set; }
        public int MotionId { get; set; }
        public string SuggestedVote { get; set; }

        public Motion Motion { get; set; }
    }
}
