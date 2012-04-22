using System.Web.Mvc;
using Core.Model;
using Core.Persistence;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly Context _context = new Context();

		public ActionResult Index()
		{
			return View(_context.Users);
		}

		public ActionResult Create(User user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
