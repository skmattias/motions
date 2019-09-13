using System;

// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class SuggestedVote
    {
        public class VoteEnum
        {
            private VoteEnum(string value) { Value = value; }
            
            public string Value { get; set; }

            public static VoteEnum Bifall => new VoteEnum("BIFALL");
            public static VoteEnum Avslag => new VoteEnum("AVSLAG");
            public static VoteEnum Instäm => new VoteEnum("INSTÄM");
        }
    }
}
