// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class AttProposition
    {
        public string FullNumber() => string.Join('.', Motion.FullNumber(), AttPropositionNumber);

        public string FullName() => FullNumber() + ": " + AttPropositionText;
    }
}
