using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Repository;

namespace WebDatMonAn.Areas.Shipper.Controllers
{
    [Area("Shipper")]
    [Authorize(AuthenticationSchemes = "ShipperScheme")]
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;
        public HomeController(DataContext dataContext,INotyfService notyfService)
        {
            _dataContext = dataContext;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            var hoaDons = _dataContext.HoaDons.Include(h => h.KhachHang).OrderByDescending(h => h.NgayDat).ToList();

            var orderDetails = _dataContext.CTHDs
                                .Include(ct => ct.MonAn)
                                .Include(ct => ct.HoaDon)
                                .Where(ct => hoaDons.Select(h => h.MaHD).Contains(ct.MaHD))
                                .ToList();

            ViewBag.HoaDons = hoaDons;
            ViewBag.OrderDetails = orderDetails;

            return View();
        }
        [HttpPost]
        public IActionResult Index(int maHD)
        {
            var hoaDon = _dataContext.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
            if (hoaDon != null)
            {
                hoaDon.TrangThaiGiaoHang = 1; 
                _dataContext.SaveChanges();
               
            }
            else
            {
                _notyfService.Error("Đơn hàng không tồn tại!");
            }
            _notyfService.Success("Xác nhận giao thành công!");

            
            return RedirectToAction("Index");
        }




    }
}
