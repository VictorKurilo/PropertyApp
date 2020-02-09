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
using TechAssesment.Core;
using TechAssesment.Core.Dto;
using TechAssesment.Core.Models;
using TechAssesment.Persistence;

namespace TechAssesment.Controllers
{
    [System.Web.Http.Authorize]
    public class PropertyController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GetAll: api/Property
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAll()
        {
            var userId = User.Identity.GetUserId();

            var properties = _unitOfWork.Properties.GetAllProperties(userId);

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

        //GetPropertyById: api/Property/{id}
        public IHttpActionResult Get(int id)
        {
            var currentProperty = _unitOfWork.Properties.GetPropertyById(id);

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
            var userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            _unitOfWork.Properties.AddProperty(propertyViewModel, userId);
            _unitOfWork.Complete();

            return Ok();
        }

        //Update: api/Property
        [System.Web.Http.HttpPut]
        public IHttpActionResult Update(PropertyViewModel propertyViewModel)
        {
            var userId = User.Identity.GetUserId();
            var existingProperty = _unitOfWork.Properties.GetPropertyById(propertyViewModel.Id);

            if (existingProperty.OwnerId != User.Identity.GetUserId())
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            if (existingProperty != null)
            {
                existingProperty.Name = propertyViewModel.Name;
                existingProperty.Description = propertyViewModel.Description;

                _unitOfWork.Complete();
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
            var currentProperty = _unitOfWork.Properties.GetPropertyById(id);

            if (id <= 0 || currentProperty == null)
                return BadRequest("not valid property");

            if (currentProperty.OwnerId != User.Identity.GetUserId())
                return Unauthorized();
           
            _unitOfWork.Properties.DeletePropertyById(currentProperty);
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
