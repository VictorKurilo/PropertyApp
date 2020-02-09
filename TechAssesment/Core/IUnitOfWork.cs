using TechAssesment.Core.Repositories;

namespace TechAssesment.Core
{
    public interface IUnitOfWork
    {
        IPropertyRepository Properties { get; }

        void Complete();
    }
}
