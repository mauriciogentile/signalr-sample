using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Solaise.Weather.Domain.Entities;
using Solaise.Weather.Domain.Events;
using Solaise.Weather.Domain.Repositories;
using Solaise.Weather.Web.Controllers.Api;
using Solaise.Weather.Web.Messaging;
using Solaise.Weather.Web.Resources.Mappings;
using Solaise.Weather.Web.Tests.Mocks;

namespace Solaise.Weather.Web.Tests
{
    [TestClass]
    public class CityController_UnitTest
    {
        ICityRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = BuildRepository();
        }

        [TestMethod]
        public void should_retrieve_all_cities_in_repository_and_OK()
        {
            var target = new CityController(_repository, new DefaultEventBus())
            {
                Request = MocksUtils.MockRequest()
            };

            IEnumerable<Resources.City> expectedContent = _repository.All().ToResource<City, Resources.City>();
            IEnumerable<Resources.City> actualContent;

            HttpResponseMessage expected = target.Request.CreateResponse(HttpStatusCode.OK, expectedContent);
            HttpResponseMessage actual = target.GetAll();

            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.IsTrue(actual.TryGetContentValue(out actualContent));
            Assert.AreEqual(expectedContent.Count(), actualContent.Count());
        }

        [TestMethod]
        public void should_post_city_in_repository_and_Created()
        {
            var target = new CityController(_repository, new DefaultEventBus())
            {
                Request = MocksUtils.MockRequest()
            };

            HttpResponseMessage expected = target.Request.CreateResponse(HttpStatusCode.Created);
            HttpResponseMessage actual = target.Post(new Resources.City { Name = "Montreal", Country = "Canada" });

            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.IsTrue(_repository.All().SingleOrDefault(x => x.Name == "Montreal" && x.Country == "Canada") != null);
        }

        [TestMethod]
        public void should_delete_city_in_repository_and_OK()
        {
            var target = new CityController(_repository, new DefaultEventBus())
            {
                Request = MocksUtils.MockRequest()
            };

            HttpResponseMessage expected = target.Request.CreateResponse(HttpStatusCode.OK);

            int expectedCount = _repository.All().Count() - 1;

            HttpResponseMessage actual = target.Delete(1);

            int actualCount = _repository.All().Count();

            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void should_publish_CityAdded_when_city_saved()
        {
            var busMock = new Mock<IEventBus>();
            busMock.Setup(x => x.Publish(It.IsAny<CityAdded>()));

            var target = new CityController(_repository, busMock.Object);

            target.Post(new Resources.City());

            busMock.Verify(x => x.Publish(It.IsAny<CityAdded>()), Times.Once);
        }

        [TestMethod]
        public void should_publish_CityRemoved_when_city_deleted()
        {
            var busMock = new Mock<IEventBus>();
            busMock.Setup(x => x.Publish(It.IsAny<CityRemoved>()));

            var target = new CityController(_repository, busMock.Object);

            target.Delete(1);

            busMock.Verify(x => x.Publish(It.IsAny<CityRemoved>()), Times.Once);
        }

        static ICityRepository BuildRepository()
        {
            var repository = new InMemoryCityRepository();
            repository.Save(new City { Id = 1, Name = "Falls Church", Country = "US" });
            repository.Save(new City { Id = 2, Name = "Vienne", Country = "France" });
            return repository;
        }
    }
}
