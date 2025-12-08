using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDatMonAn.Area.Admin.Controllers
{
    [Area("Admin")]
  
    public class QuanTriController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
