using System.Collections.Generic;
using System.Linq;
using Solaise.Weather.Domain.Entities;

namespace Solaise.Weather.Domain.Repositories
{
    public sealed class InMemoryCityRepository : ICityRepository
    {
        private readonly Dictionary<int, City> _internalRepo = new Dictionary<int, City>();

        public IEnumerable<City> All()
        {
            return _internalRepo.Values.ToList();
        }

        public void Save(City city)
        {
            if (_internalRepo.ContainsKey(city.Id))
            {
                _internalRepo[city.Id] = city;
                return;
            }
            if (city.Id == 0)
            {
                city.Id = _internalRepo.Count + 1;
            }
            _internalRepo.Add(city.Id, city);
        }

        public void Delete(int id)
        {
            _internalRepo.Remove(id);
        }
    }
}
