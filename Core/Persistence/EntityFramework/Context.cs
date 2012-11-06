using Core.Model;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Core.Persistence.EntityFramework
{
	public class Context : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			ConfigureModel(modelBuilder);

			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());

			base.OnModelCreating(modelBuilder);
		}

		private void ConfigureModel(DbModelBuilder modelBuilder)
		{
			var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

			var entityTypes = Assembly.GetAssembly(typeof(Entity)).GetTypes()
				.Where(x => x.IsSubclassOf(typeof(Entity)) && !x.IsAbstract);
			foreach (var type in entityTypes)
			{
				entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
			}
		}
	}
}
