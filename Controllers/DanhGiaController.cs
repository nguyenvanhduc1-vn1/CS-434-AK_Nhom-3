using Microsoft.AspNetCore.Mvc;

namespace WebDatMonAn.Controllers
{
	public class DanhGiaController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
