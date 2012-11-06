using Core.Model;
using Core.Persistence;
using Core.Persistence.EntityFramework;
using System.Web.Mvc;

namespace Web.Controllers
{
	public class HibernatedController : Controller
	{
		private readonly IRepository _repository;

		public HibernatedController(NHibernateRepository repository)
		{
			_repository = repository;
		}

		public ActionResult Index()
		{
			return View(_repository.GetAll<User>());
		}

		public ActionResult Create(User user)
		{
			_repository.SaveOrUpdate(user);

			return RedirectToAction("Index");
		}
	}
}
