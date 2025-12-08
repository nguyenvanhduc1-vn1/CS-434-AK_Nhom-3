using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;
using X.PagedList;

namespace WebDatMonAn.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyfService;

        public HomeController(ILogger<HomeController> logger, DataContext context,INotyfService notyfService)
        {
            _dataContext = context;
            _logger = logger;
            _notyfService = notyfService;
        }

        public IActionResult Index()

        {
            
            var monan = _dataContext.MonAns.Include(c => c.DanhMuc).AsNoTracking().Where(d => d.TrangThai == 1).OrderBy(x => x.NgayTao).Take(8).ToList();
            return View(monan);
        }
        public IActionResult TimKiem(string? query)
        {
            var monans = _dataContext.MonAns.Include(c => c.DanhMuc).AsQueryable();
            if(query != null)
            {
                monans = monans.Where(p => p.TenMonAn.Contains(query)
                || p.DonGia.ToString().Contains(query) ||
                p.DanhMuc.TenDanhMuc.Contains(query));
            }
			_notyfService.Success("kết quả tìm kiếm thành công!");
			return View(monans.ToList());
        }
         
        public async  Task<IActionResult> Detail( int Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var monan =   _dataContext.MonAns.Include(x => x.DanhMuc).Where(p => p.MaMonAn == Id).FirstOrDefault();
			_notyfService.Success("truy cập  thành công!");
			return View(monan);
        }

		public IActionResult Tatcamonan(int? page)
		{
			var pageNumber = page ?? 1; // Default to the first page if page is null
			var pageSize = 6;

			var monan = _dataContext.MonAns
				.Include(c => c.DanhMuc)
				.AsNoTracking()
				.OrderBy(x => x.NgayTao)
				.Where(d => d.TrangThai == 1)
				.ToList();

			
			var pagedMonan = new PagedList<MonAnModel>(monan, pageNumber, pageSize);

			ViewBag.CurrentPage = pageNumber;
			return View("Tatcamonan",pagedMonan); 
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
