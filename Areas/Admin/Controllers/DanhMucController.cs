using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;

namespace WebDatMonAn.Area.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class DanhMucController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;

        public DanhMucController(DataContext dataContext, INotyfService notyfService)
        {
            _notyfService = notyfService;
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _notyfService.Success("Truy cập thành công!");
            var danhmuc = _dataContext.DanhMucs.OrderByDescending(c => c.MaDanhMuc).ToList();
            return View(danhmuc);
        }

        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucModel danhMuc)
        {
            danhMuc.SlugDanhMuc = danhMuc.TenDanhMuc.Replace(" ", "-");
            var tendanhmuc = await _dataContext.DanhMucs.FirstOrDefaultAsync(p => p.TenDanhMuc == danhMuc.TenDanhMuc);
            if (tendanhmuc != null)
            {
                _notyfService.Error("Danh mục đã có trong đã tồn tại!");
                return View(danhMuc);
            }
            _dataContext.Add(danhMuc);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Thêm mới danh mục thành công!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var danhmuc = await _dataContext.DanhMucs.FindAsync(Id);
            if (danhmuc == null)
            {
                return NotFound(); // Return 404 if not found
            }
            return View(danhmuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DanhMucModel danhmuc)
        {
            danhmuc.SlugDanhMuc = danhmuc.TenDanhMuc.Replace(" ", "-");
            var tendanhmuc = await _dataContext.DanhMucs.FirstOrDefaultAsync(p => p.TenDanhMuc == danhmuc.TenDanhMuc);
            var exists_category = await _dataContext.DanhMucs.FindAsync(danhmuc.MaDanhMuc);
            if (tendanhmuc != null && tendanhmuc.MaDanhMuc != danhmuc.MaDanhMuc)
            {
                _notyfService.Error("Danh mục đã có trong đã tồn tại!");
                return View(danhmuc);
            }
            if (exists_category == null)
            {
                return NotFound(); 
            }
            exists_category.TenDanhMuc = danhmuc.TenDanhMuc;
            exists_category.MoTa = danhmuc.MoTa;
            exists_category.TrangThai = danhmuc.TrangThai;
            _dataContext.Update(exists_category);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Cập nhật danh mục thành công!");
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Delete(int Id)
        {
            var danhMuc = await _dataContext.DanhMucs.FindAsync(Id);
            if (danhMuc == null)
            {
                return NotFound(); 
            }
            _dataContext.DanhMucs.Remove(danhMuc);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Xóa danh mục thành công!");
            return RedirectToAction("Index");
        }
    }
}
