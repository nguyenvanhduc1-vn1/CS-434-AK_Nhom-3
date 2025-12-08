using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;
using WebDatMonAn.Repository.Extension;

namespace WebDatMonAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class ShipperController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;

        public ShipperController(DataContext dataContext,INotyfService notyfService)
        {
            _dataContext = dataContext;
            _notyfService = notyfService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var shipper = _dataContext.Shippers.ToList();
            return View(shipper);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipperModel shipper)
        { 

            var tenDN = await _dataContext.Shippers.FirstOrDefaultAsync(p => p.TenDN == shipper.TenDN);
            if (tenDN != null)
            {
                _notyfService.Error("tên Tài khoản đã tồn tại!");
                return View(shipper);
            }
            shipper.MatKhau = shipper.MatKhau.Trim().ToMD5();
            _dataContext.Add(shipper);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Thêm mới  thành công!");
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var danhmuc = await _dataContext.Shippers.FindAsync(Id);
            return View(danhmuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShipperModel shipper)
        {
           
            var existingShipper = await _dataContext.Shippers
                .Where(p => p.TenDN == shipper.TenDN && p.MaShip != shipper.MaShip)
                .FirstOrDefaultAsync();
            if (existingShipper != null)
            {
                _notyfService.Error("Tên tài khoản đã tồn tại!");
                return View(shipper);
            }

            var shippers = await _dataContext.Shippers.FindAsync(shipper.MaShip);
            if (shippers == null)
            {
                _notyfService.Error("Shipper không tồn tại!");
                return RedirectToAction("Index");
            }

            shippers.TenDN = shipper.TenDN;
            shippers.MatKhau = shipper.MatKhau.Trim().ToMD5(); 
            shippers.SoDienThoai = shipper.SoDienThoai;
            shippers.Email = shipper.Email;

            
            _dataContext.Shippers.Update(shippers);
            await _dataContext.SaveChangesAsync();

            _notyfService.Success("Cập nhật thành công!");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            ShipperModel shipper = await _dataContext.Shippers.FindAsync(Id);
            _dataContext.Shippers.Remove(shipper);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Xóa  thành công!");
            return RedirectToAction("Index");
        }
    }
}
