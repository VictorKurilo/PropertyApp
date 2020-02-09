using System.Data.Entity;
using TechAssesment.Core.Models;

namespace TechAssesment.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Property> Properties { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }
    }
}