using System.Collections.Generic;
using Solaise.Weather.Domain.Entities;

namespace Solaise.Weather.Domain.Repositories
{
    public interface ICityRepository
    {
        IEnumerable<City> All();
        void Save(City city);
        void Delete(int id);
    }
}