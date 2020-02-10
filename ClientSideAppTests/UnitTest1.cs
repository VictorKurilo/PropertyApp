using System;
using ClientSideApp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientSideAppTests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            var controller = new PropertyController()
            {

            };
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange.
            const int locationId = 1;
            const LocationType locationType = LocationType.City;
            int pageSize = 10;

            // Act.
            var postsPageOne = PostService.FindAllPostsForLocationPaged<Review>(locationId, locationType, 1, pageSize);
            var postsPageTwo = PostService.FindAllPostsForLocationPaged<Review>(locationId, locationType, 2, pageSize);

            // Assert.
            Assert.IsTrue(postsPageOne.Count > 0);
            Assert.IsTrue(postsPageTwo.Count > 0);
            Assert.AreEqual(postsPageOne.Count, pageSize);
            Assert.AreEqual(postsPageTwo.Count, pageSize);
            CollectionAssert.AllItemsAreNotNull(postsPageOne.ToArray());
            CollectionAssert.AllItemsAreNotNull(postsPageTwo.ToArray());
        }
    }
}
