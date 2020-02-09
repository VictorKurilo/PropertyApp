using System.Collections.Generic;
using TechAssesment.Controllers;
using TechAssesment.Core.Models;

namespace TechAssesment.Core.Repositories
{
    public interface IPropertyRepository
    {
        List<Property> GetAllProperties(string userId);

        Property GetPropertyById(int id);

        void AddProperty(PropertyViewModel propertyViewModel, string userId);

        void DeletePropertyById(Property currentProperty);
    }
}