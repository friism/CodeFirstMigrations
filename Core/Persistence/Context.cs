using System.Data.Entity;

namespace Core.Persistence
{
	public class Context : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
		}
	}
}
