using System.Collections.Generic;

namespace ClientSideApp.Services
{
    public interface IPropertyService
    {
        string GetProperties();

        string GetPropertyById(int id);
        void CreateProperty(List<KeyValuePair<string, string>> keyValuePairs);
        void DeletePropertyById(int id);
        void UpdateProperty(List<KeyValuePair<string, string>> keyValuePairs);
    }
}