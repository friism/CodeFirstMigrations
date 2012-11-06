using Core.Model;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace Core.Persistence.EntityFramework
{
	public class NHibernateRepository : IRepository, IDisposable
	{
		private readonly ISession _session;

		public NHibernateRepository(ISession session)
		{
			_session = session;
		}

		public T Get<T>(int id) where T : Entity
		{
			return _session.Get<T>(id);
		}

		public IQueryable<T> GetAll<T>() where T : Entity
		{
			return _session.Query<T>();
		}

		public void SaveOrUpdate<T>(T entity) where T : Entity
		{
			using (var transaction = _session.BeginTransaction())
			{
				_session.SaveOrUpdate(entity);
				transaction.Commit();
			}
		}

		public void Delete<T>(T entity) where T : Entity
		{
			using (var transaction = _session.BeginTransaction())
			{
				_session.Delete(entity);
				transaction.Commit();
			}
		}

		public void Dispose()
		{
			_session.Dispose();
		}
	}
}
