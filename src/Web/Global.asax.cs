using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Solaise.Weather.Domain.Events;
using Solaise.Weather.Domain.Repositories;
using Solaise.Weather.Web.Messaging;
using Solaise.Weather.Web.Messaging.WebSockets;
using Solaise.Weather.Web.Resources;

namespace Solaise.Weather.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            WebApiConfig.RegisterFormatters(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureIoCContainer();

            var resolver = (AutofacWebApiDependencyResolver)GlobalConfiguration.Configuration.DependencyResolver;

            var bus = resolver.Container.Resolve<IEventBus>();
            var cityEventsPublisher = resolver.Container.Resolve<ICityEventsPublisher>();
            var cityRepository = resolver.Container.Resolve<ICityRepository>();
            cityRepository.Save(new Domain.Entities.City { Name = "Falls Church", Country = "US" });
            cityRepository.Save(new Domain.Entities.City { Name = "London", Country = "UK" });
            cityRepository.Save(new Domain.Entities.City { Name = "Birmingham", Country = "UK" });
            cityRepository.Save(new Domain.Entities.City { Name = "Manchester", Country = "UK" });
            cityRepository.Save(new Domain.Entities.City { Name = "Vienne", Country = "FR" });

            bus.Subscribe<CityAdded>(e => cityEventsPublisher.CityAdded(new WebSocketEvent<CityAdded>(e)));
            bus.Subscribe<CityRemoved>(e => cityEventsPublisher.CityRemoved(new WebSocketEvent<CityRemoved>(e)));
        }

        static void ConfigureIoCContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            builder.RegisterType<DefaultEventBus>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance(CityEventsPublisher.Instance).AsImplementedInterfaces();
            builder.RegisterInstance(new InMemoryCityRepository()).AsImplementedInterfaces().SingleInstance();
            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}