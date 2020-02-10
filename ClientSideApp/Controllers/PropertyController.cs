using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClientSideApp.Models;
using ClientSideApp.Services;
using ClientSideApp.ViewModels;
using Newtonsoft.Json;

namespace ClientSideApp.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {

        private readonly IPropertyService _propertyService;

        public PropertyController()
        {
            _propertyService = new PropertyService(new HttpClient());
        }

        // GET: Property
        public ActionResult Index()
        {
            var response = _propertyService.GetProperties();

            var properties = JsonConvert.DeserializeObject<ICollection<PropertyViewModel>>(response);

            return View(properties);
        }

        // GET: Property/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Property/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Property/Create
        [HttpPost]
        public ActionResult Create(PropertyViewModel model)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {"Name", model.Name },
                {"Description", model.Description },
            };

            try
            {
                _propertyService.CreateProperty(new List<KeyValuePair<string, string>> (dictionary));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Property/Edit/5
        public ActionResult Edit(int id)
        {
            var response = _propertyService.GetPropertyById(id);

            var property = JsonConvert.DeserializeObject<PropertyViewModel>(response);

            return View("Edit", property);
        }

        // POST: Property/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PropertyViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                { "Id", id.ToString() },
                {"Name", model.Name },
                {"Description", model.Description },
            };

            try
            {
                _propertyService.UpdateProperty(new List<KeyValuePair<string, string>>(dictionary));

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: Property/Delete/5
        public ActionResult Delete(int id)
        {
            var response = _propertyService.GetPropertyById(id);

            var property = JsonConvert.DeserializeObject<PropertyViewModel>(response);

            return View(property);
        }

        // POST: Property/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, PropertyViewModel model)
        {
            try
            {
                _propertyService.DeletePropertyById(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
