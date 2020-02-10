using System.Collections.Generic;
using ClientSideApp.ViewModels;

namespace ClientSideApp.Repositories
{
    public interface IPropertyRepository
    {
        ICollection<PropertyViewModel> GetPropertyList();
    }
}