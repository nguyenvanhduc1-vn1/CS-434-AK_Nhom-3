using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;
using X.PagedList;

namespace WebDatMonAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class KhachHangController : Controller
    {
        private readonly DataContext _dataContext;
        public KhachHangController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var dskhachhang = _dataContext.KhachHangs.AsNoTracking()
                                    .Include(x=>x.DiaDiem)
                                    .OrderByDescending(x => x.MaKH);
            PagedList<KhachHangModel> models = new PagedList<KhachHangModel>(dskhachhang, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
    }
}
