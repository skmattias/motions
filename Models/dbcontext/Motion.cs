using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Motion
    {
        public Motion()
        {
            AttProposition = new HashSet<AttProposition>();
        }

        public int Id { get; set; }
        public int MotionNumber { get; set; }
        public string MotionName { get; set; }
        public string MotionText { get; set; }
        public int CommitteeId { get; set; }

        public Committee Committee { get; set; }
        public ICollection<AttProposition> AttProposition { get; set; }
    }
}
