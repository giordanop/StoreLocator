using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Owin;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.Mvc;
using Ninject.Web.WebApi.OwinHost;
using StoreLocator.Domain.Caching;
using StoreLocator.Domain.Entities;
using StoreLocator.Domain.Repository;
using StoreLocator.Domain.Setting;
using StoreLocator.Web.Api;
using StoreLocator.Web.Api.Models;

[assembly: OwinStartup(typeof(Startup))]

namespace StoreLocator.Web.Api
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            app.UseNinjectMiddleware(CreateKernel);
            app.UseNinjectWebApi(configuration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<StoreLocatorEntities>().ToSelf().InRequestScope();
            kernel.Bind<IDomainSettings>().To<DomainSettingsCacheEnabled>();
            kernel.Bind<ICacheProvider>().To<AspNetCacheProvider>();
            kernel.Bind<ModelFactory>().To<ModelFactory>();
            kernel.Bind<ILocationRepository>().To<LocationRepository>();
            return kernel;
        }
    }
}