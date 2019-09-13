﻿using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class SuggestedVote
    {
        public int Id { get; set; }
        public int AttId { get; set; }
        public string Vote { get; set; }
        public int ActorId { get; set; }

        public Actor Actor { get; set; }
        public Att Att { get; set; }
    }
}
