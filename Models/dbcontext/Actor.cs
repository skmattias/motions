using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Actor
    {
        public Actor()
        {
            Response = new HashSet<Response>();
            SuggestedVote = new HashSet<SuggestedVote>();
        }

        public int Id { get; set; }
        public string ActorName { get; set; }

        public ICollection<Response> Response { get; set; }
        public ICollection<SuggestedVote> SuggestedVote { get; set; }
    }
}
