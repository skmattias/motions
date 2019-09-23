// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class Att
    {
        public string FullNumber()
        {
            return IsPsAtt() ? "" : string.Join('.', Motion.FullNumber(), AttNumber);
        }

        public string FullName()
        {
            return IsPsAtt() ? "" : FullNumber() + ": " + AttText;
        }
        
        public bool IsPsAtt() => AttNumber == -1;
    }
}
