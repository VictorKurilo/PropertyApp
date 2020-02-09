using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Web;
using TechAssesment.Controllers;
using TechAssesment.Core.Models;
using TechAssesment.Core.Repositories;

namespace TechAssesment.Persistence.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IApplicationDbContext _context;

        public PropertyRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public List<Property> GetAllProperties(string userId)
        {
            return _context.Properties.Where(property => property.OwnerId == userId).ToList();
        }

        public Property GetPropertyById(int id)
        {
            return _context.Properties.FirstOrDefault(property => property.Id == id);
        }

        public void AddProperty(PropertyViewModel propertyViewModel, string userId)
        {
            _context.Properties.Add(new Property()
            {
                Name = propertyViewModel.Name,
                Description = propertyViewModel.Description,
                OwnerId = userId
            });
        }

        public void DeletePropertyById(Property currentProperty)
        {
            _context.Properties.Remove(currentProperty);
        }
    }
}