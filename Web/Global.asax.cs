using Core.Model;
using Core.Persistence.EntityFramework;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StructureMap;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Mvc;
using NHConfig = NHibernate.Cfg;

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

				x.For<EntityFrameworkRepository>()
					.HttpContextScoped()
					.Use(context => new EntityFrameworkRepository(context.GetInstance<Context>()));

				x.For<ISessionFactory>()
					.Singleton()
					.Use(CreateSessionFactory());

				x.For<ISession>()
					.HttpContextScoped()
					.Use(context => context.GetInstance<ISessionFactory>().OpenSession());

				x.For<NHibernateRepository>()
					.HttpContextScoped()
					.Use(context => new NHibernateRepository(context.GetInstance<ISession>()));
			});

			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
		}

		protected ISessionFactory CreateSessionFactory()
		{
			var autoMap = AutoMap.AssemblyOf<Entity>()
				.Where(t => typeof(Entity).IsAssignableFrom(t));

			return Fluently.Configure()
				.Database(
					MsSqlCeConfiguration.Standard.ConnectionString(x => x.FromConnectionStringWithKey("context"))
				)
				.Mappings(m => m.AutoMappings.Add(autoMap))
				.ExposeConfiguration(TreatConfiguration)
				.BuildSessionFactory();
		}

		protected virtual void TreatConfiguration(NHConfig.Configuration configuration)
		{
			var update = new SchemaUpdate(configuration);
			update.Execute(false, true);
		}
	}
}
