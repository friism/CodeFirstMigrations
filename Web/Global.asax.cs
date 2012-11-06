using Core.Persistence;
using Core.Persistence.EntityFramework;
using StructureMap;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Mvc;

namespace Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

			ObjectFactory.Initialize(x =>
			{
				x.For<Context>()
					.Use(new Context());

				x.For<IRepository>()
					.HttpContextScoped()
					.Use(context => new EntityFrameworkRepository(context.GetInstance<Context>()));
			});

			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
		}
	}
}
