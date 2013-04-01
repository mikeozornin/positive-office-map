using System.Web.Mvc;

namespace MapMvc.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}
	}
}
