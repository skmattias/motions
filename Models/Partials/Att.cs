// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class Att
    {
        public string FullNumber() => string.Join('.', Motion.FullNumber(), AttNumber);

        public string FullName() => FullNumber() + ": " + AttText;
    }
}
