using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace CsAspnet.Models.dbcontext
{
    public partial class Actor
    {
        public static async Task<Actor> GetCsAsync(motionsContext context)
        {
            return await context.Actor.SingleOrDefaultAsync(a => a.ActorName == "Centerstudenter");
        }

        public static async Task<Actor> GetPsAsync(motionsContext context)
        {
            return await context.Actor.SingleOrDefaultAsync(a => a.ActorName == "Partistyrelsen");
        }
    }
}
