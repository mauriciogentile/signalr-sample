using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Solaise.Weather.Domain.Events;
using Solaise.Weather.Domain.Repositories;
using Solaise.Weather.Web.Messaging;
using Solaise.Weather.Web.Resources;
using Solaise.Weather.Web.Resources.Mappings;

namespace Solaise.Weather.Web.Controllers.Api
{
    public class CityController : BaseController
    {
        readonly ICityRepository _repository;
        readonly IEventBus _bus;

        public CityController()
            : this(new InMemoryCityRepository(), new DefaultEventBus())
        {
        }

        public CityController(ICityRepository repository, IEventBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        public HttpResponseMessage GetAll()
        {
            return Get(() =>
            {
                var cities = _repository.All();
                return cities.ToResource<Domain.Entities.City, City>();
            });
        }

        [HttpPost]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            return Delete(() =>
            {
                _repository.Delete(id);
                _bus.Publish(new CityRemoved(id));
            });
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] City city)
        {
            return Post(() =>
            {
                var newCity = city.ToEntity<City, Domain.Entities.City>();
                _repository.Save(newCity);
                _bus.Publish(new CityAdded(newCity));
            });
        }
    }
}
