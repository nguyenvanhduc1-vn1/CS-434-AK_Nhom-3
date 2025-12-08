using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;
using WebDatMonAn.Repository.Extension;

namespace WebDatMonAn.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]

    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;
        public AccountController(DataContext dataContext,INotyfService notyfService)
        {
            _dataContext = dataContext;
            _notyfService = notyfService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_dataContext.PhanQuyens, "MaCN", "TenQuyen");
            var account = _dataContext.CTCNs.Include(c => c.NhanVien).Include(c => c.ChucNang);

            return View(await account.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PhanQuyens = new SelectList(_dataContext.PhanQuyens, "MaCN", "TenQuyen");
            ViewBag.NhanViens = new SelectList(_dataContext.NhanViens, "MaNV", "TenNV");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Create(ChiTietChucNang CTCN)
        {
                _dataContext.Add(CTCN);
                await _dataContext.SaveChangesAsync();
                 _notyfService.Success("Phân quyền thành công!");
                return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult ThemNV()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemNV(NhanVienModel nhanvien)
        {
            var tennhanvien = await _dataContext.NhanViens.FirstOrDefaultAsync(p => p.TenNV == nhanvien.TenNV);
            if (tennhanvien != null)
            {
                _notyfService.Error("Tên Người truy cập đã tồn tại");
                return View(nhanvien);
            }

           
            nhanvien.MatKhau = nhanvien.MatKhau.Trim().ToMD5();
            _dataContext.NhanViens.Add(nhanvien);
            await _dataContext.SaveChangesAsync();

            _notyfService.Success("Thêm mới thành công!");
            return RedirectToAction("Index");
        }
    }
}
