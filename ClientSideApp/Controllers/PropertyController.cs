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
using ClientSideApp.Repositories;
using ClientSideApp.Services;
using ClientSideApp.ViewModels;
using Newtonsoft.Json;
using PagedList;

namespace ClientSideApp.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyRepository _propertyRepository;

        public PropertyController(IPropertyRepository propertyRepository)
        {
            _propertyService = new PropertyService(new HttpClient());
            _propertyRepository = propertyRepository;
        }

        // GET: Property
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var properties = _propertyRepository.GetPropertyList();

            if (properties == null)
                return View();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var homes = from property in properties
                           select property;
            if (!String.IsNullOrEmpty(searchString))
            {
                homes = homes.Where(home => home.Name.Contains(searchString)
                                               || home.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    homes = homes.OrderByDescending(s => s.Name);
                    break;
                default:  // Name ascending 
                    homes = homes.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(homes.ToPagedList(pageNumber, pageSize));
        }



        // GET: Property/Details/5
        public ActionResult Details(int id)
        {
            var response = _propertyService.GetPropertyById(id);

            var property = JsonConvert.DeserializeObject<PropertyViewModel>(response);

            return View(property);
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
