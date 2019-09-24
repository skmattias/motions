using System;
using System.Collections.Generic;

namespace CsAspnet.Models.dbcontext
{
    public partial class Section
    {
        public int Id { get; set; }
        public int Placement { get; set; }
        public int TitleLevel { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public int ProgramId { get; set; }

        public Program Program { get; set; }
    }
}
