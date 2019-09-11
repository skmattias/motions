// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class Motion
    {
        public string FullNumber() => string.Join('.', Committee.CommitteeNumber, MotionNumber);

        public string FullName() => FullNumber() + ": " + MotionName;
    }
}
