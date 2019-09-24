using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Program
    {
        public Program()
        {
            Att = new HashSet<Att>();
            Section = new HashSet<Section>();
        }

        public int Id { get; set; }
        public int ProgramNumber { get; set; }
        public string ProgramTitle { get; set; }

        public ICollection<Att> Att { get; set; }
        public ICollection<Section> Section { get; set; }
    }
}
