// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class Committee
    {
        public string FullName() => CommitteeNumber + ": " + CommitteeName;
    }
}