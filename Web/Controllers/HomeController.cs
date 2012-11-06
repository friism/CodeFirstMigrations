using Core.Model;
using Core.Persistence;
using System.Web.Mvc;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IRepository _repository;

		public HomeController(IRepository repository)
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
