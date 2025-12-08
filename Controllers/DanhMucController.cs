using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;
using X.PagedList;

namespace WebDatMonAn.Controllers
{

    public class DanhMucController : Controller
    {
        private readonly DataContext _dataContext;
        public DanhMucController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string Slug = "", int? page = 1)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            DanhMucModel danhmuc = _dataContext.DanhMucs.Where(c => c.SlugDanhMuc == Slug).FirstOrDefault();
            if (danhmuc == null) return RedirectToAction("Index");

            var monantheodanhmuc = _dataContext.MonAns
                .Include(c => c.DanhMuc)
                .Where(p => p.MaDanhMuc == danhmuc.MaDanhMuc)
                .OrderByDescending(p => p.MaDanhMuc);

            var pagedMonan = await monantheodanhmuc.ToPagedListAsync(pageNumber, pageSize);

            return View("~/Views/Home/Tatcamonan.cshtml", pagedMonan);
        }

    }
}
