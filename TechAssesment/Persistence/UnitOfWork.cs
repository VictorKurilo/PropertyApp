using TechAssesment.Core;
using TechAssesment.Core.Repositories;
using TechAssesment.Persistence.Repositories;

namespace TechAssesment.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IPropertyRepository Properties { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Properties = new PropertyRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}