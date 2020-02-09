using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TechAssesment.Dto;
using TechAssesment.Models;

namespace TechAssesment.Controllers
{
    [System.Web.Http.Authorize]
    public class PropertyController : ApiController
    {

        private readonly ApplicationDbContext _context;

        public PropertyController()
        {
            _context = new ApplicationDbContext();
        }

        //GetAll: api/Property
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAll()
        {
            var userId = User.Identity.GetUserId();

            var properties = _context.Properties.Where(property => property.OwnerId == userId).ToList();

            ICollection<PropertyDto> propertyDtos = new List<PropertyDto>();

            foreach (var property in properties)
            {
                propertyDtos.Add(new PropertyDto()
                {
                    Id = property.Id,
                    Name = property.Name,
                    Description = property.Description,
                });
            }

            if (propertyDtos.Count == 0)
                return NotFound();

            return Ok(propertyDtos);
        }

        //Get: api/Property/{id}
        public IHttpActionResult Get(int id)
        {
            var currentProperty = _context.Properties.SingleOrDefault(property => property.Id == id);

            if (currentProperty == null)
                return NotFound();

            if (currentProperty.OwnerId != User.Identity.GetUserId())
                return Unauthorized();

            PropertyDto propertyDto = new PropertyDto();

            propertyDto.Id = currentProperty.Id;
            propertyDto.Name = currentProperty.Name;
            propertyDto.Description = currentProperty.Description;

            return Ok(propertyDto);
        }

        //Post: api/Property
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
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

        //Update: api/Property
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

        //Delete: api/Property/{id}
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
