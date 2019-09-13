using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Response
    {
        public int Id { get; set; }
        public string ResponseText { get; set; }
        public int ActorId { get; set; }
        public int MotionId { get; set; }

        public Actor Actor { get; set; }
        public Motion Motion { get; set; }
    }
}
