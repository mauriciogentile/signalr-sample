using System.Collections.Generic;
using AutoMapper;
using Solaise.Weather.Domain.Entities;

namespace Solaise.Weather.Web.Resources.Mappings
{
    public static class MappingExtensions
    {
        static MappingExtensions()
        {
            Mapper.CreateMap<IEnumerable<City>, IEnumerable<Domain.Entities.City>>();
            Mapper.CreateMap<City, Domain.Entities.City>();
            Mapper.CreateMap<Domain.Entities.City, City>();
        }

        public static IEnumerable<TOut> ToResource<TIn, TOut>(this IEnumerable<TIn> entities) where TIn : Entity
        {
            return Mapper.Map<IEnumerable<TOut>>(entities);
        }

        public static TOut ToResource<TIn, TOut>(this TIn entity) where TIn : Entity
        {
            return Mapper.Map<TOut>(entity);
        }

        public static TOut ToEntity<TIn, TOut>(this TIn resource) where TIn : Resource
        {
            return Mapper.Map<TOut>(resource);
        }
    }
}