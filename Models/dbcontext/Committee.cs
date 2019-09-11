using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Committee
    {
        public Committee()
        {
            Motion = new HashSet<Motion>();
        }

        public int Id { get; set; }
        public string CommitteeName { get; set; }
        public int CommitteeNumber { get; set; }
        public string Description { get; set; }

        public ICollection<Motion> Motion { get; set; }
    }
}
