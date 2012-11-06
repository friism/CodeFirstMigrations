using Core.Model;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Core.Persistence.EntityFramework
{
	public class EntityFrameworkRepository : IRepository, IDisposable
	{
		private Context _context;
		private readonly ConcurrentDictionary<Type, object> _dbSets =
			new ConcurrentDictionary<Type, object>();

		public EntityFrameworkRepository(Context context)
		{
			_context = context;
		}

		public T Get<T>(int id) where T : Entity
		{
			return GetDbSet<T>().Find(id);
		}

		public IQueryable<T> GetAll<T>() where T : Entity
		{
			return GetDbSet<T>();
		}

		public void SaveOrUpdate<T>(T entity) where T : Entity
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				GetDbSet<T>().Add(entity);
			}
			else
			{
				_context.Entry(entity).State = EntityState.Modified;
			}

			_context.SaveChanges();
		}

		public void Delete<T>(T entity) where T : Entity
		{
			GetDbSet<T>().Remove(entity);
			_context.SaveChanges();
		}

		private DbSet<T> GetDbSet<T>() where T : Entity
		{
			return (DbSet<T>)_dbSets.GetOrAdd(typeof(T), x => _context.Set<T>());
		}

		public void Dispose()
		{
			if (_context != null)
			{
				_context.Dispose();
				_context = null;
			}
		}
	}
}
