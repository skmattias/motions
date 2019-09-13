using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Motion
    {
        public Motion()
        {
            Att = new HashSet<Att>();
            Response = new HashSet<Response>();
        }

        public int Id { get; set; }
        public int MotionNumber { get; set; }
        public string MotionName { get; set; }
        public string MotionText { get; set; }
        public int CommitteeId { get; set; }

        public Committee Committee { get; set; }
        public ICollection<Att> Att { get; set; }
        public ICollection<Response> Response { get; set; }
    }
}
