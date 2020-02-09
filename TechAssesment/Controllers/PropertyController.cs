using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using TechAssesment.Models;

namespace TechAssesment.Controllers
{
    [Authorize]
    public class PropertyController : ApiController
    {

        private readonly ApplicationDbContext _context;

        public PropertyController()
        {
            _context = new ApplicationDbContext();
        }

        // api/Property
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAll()
        {
            var userId = User.Identity.GetUserId();

            var properties = _context.Properties.Where(property => property.OwnerId == userId).ToList();
            
            if (properties.Count == 0)
                return NotFound();

            return Ok(properties);
        }

        public IHttpActionResult Get(int id)
        {
            var currentProperty = _context.Properties.SingleOrDefault(property => property.Id == id);
            
            if (currentProperty == null)
                return NotFound();

            return Ok(currentProperty);
        }

        // api/Property
        [System.Web.Http.HttpPost]
        public IHttpActionResult Add(PropertyViewModel propertyViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            _context.Properties.Add(new Property()
            {
                Name = propertyViewModel.Name,
                Description = propertyViewModel.Description,
                OwnerId = User.Identity.GetUserId()
            });

            _context.SaveChanges();

            return Ok();
        }

        [System.Web.Http.HttpPut]
        public IHttpActionResult Update(PropertyViewModel propertyViewModel)
        {
            var userId = User.Identity.GetUserId();
            var existingProperty = _context.Properties.FirstOrDefault(property => property.Id == propertyViewModel.Id);

            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            if (existingProperty != null)
            {
                existingProperty.Name = propertyViewModel.Name;
                existingProperty.Description = propertyViewModel.Description;

                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            var currentProperty = _context.Properties.FirstOrDefault(property => property.Id == id);

            if (id <= 0 || currentProperty == null)
                return BadRequest("not valid property");

            if (currentProperty.OwnerId != User.Identity.GetUserId())
                return Unauthorized();
           

            _context.Entry(currentProperty).State = EntityState.Deleted;
            _context.SaveChanges();

            return Ok();
        }
    }
}
