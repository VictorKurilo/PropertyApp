using System.Collections.Generic;
using ClientSideApp.Services;
using ClientSideApp.ViewModels;
using Newtonsoft.Json;

namespace ClientSideApp.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IPropertyService _propertyService;

        public PropertyRepository(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public ICollection<PropertyViewModel> GetPropertyList()
        {
            var response = _propertyService.GetProperties();

            var properties = JsonConvert.DeserializeObject<ICollection<PropertyViewModel>>(response);
            return properties;
        }
    }
}