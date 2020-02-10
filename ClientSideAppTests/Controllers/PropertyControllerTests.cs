using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ClientSideApp.Repositories;
using ClientSideApp.ViewModels;
using FluentAssertions;
using Moq;

namespace ClientSideApp.Controllers.Tests
{
    [TestClass]
    public class PropertyControllerTests
    {
        private PropertyController _controller;
        private Mock<IPropertyRepository> _mockService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockService = new Mock<IPropertyRepository>();
            _controller = new PropertyController(_mockService.Object);
        }



        [TestMethod]
        public void Attend_UserAttendingAGigForWhichHeHasAnAttendance_ShouldReturnBadRequest()
        {
            var properties = new List<PropertyViewModel>()
            {
                new PropertyViewModel(){Id = 1, Name = "example1", Description = "example1"},
                new PropertyViewModel(){Id = 2, Name = "example2", Description = "example2"},
                new PropertyViewModel(){Id = 3, Name = "example3", Description = "example3"},
                new PropertyViewModel(){Id = 4, Name = "example4", Description = "example4"},
                new PropertyViewModel(){Id = 5, Name = "example5", Description = "example5"},
                new PropertyViewModel(){Id = 6, Name = "example6", Description = "example6"},
            };

            _mockService.Setup(r => r.GetPropertyList()).Returns(properties);

            var result = _controller.Index("name_desc", "", "", 5);

            result.Should().Be(properties);
        }
    }
}