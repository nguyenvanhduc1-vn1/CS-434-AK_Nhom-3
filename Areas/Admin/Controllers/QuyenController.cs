using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;

namespace WebDatMonAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class QuyenController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;
        public QuyenController(DataContext dataContext, INotyfService notyfService)
        {
            _dataContext = dataContext;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.PhanQuyens.ToListAsync());
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChucNangModel chucNang)
        {
            var tenquyen = await _dataContext.PhanQuyens.FirstOrDefaultAsync(d => d.TenQuyen == chucNang.TenQuyen);
            if (tenquyen != null)
            {
                _notyfService.Error("Tên quyền đã có trong database");
                return View();
            }
            _dataContext.Add(chucNang);
            _dataContext.SaveChanges();
            _notyfService.Success("Thêm mới quyền truy cập thành công");

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ChucNangModel chucNang = await _dataContext.PhanQuyens.FindAsync(Id);
            _notyfService.Success("Lấy dữ liệu thành công");
            return View(chucNang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChucNangModel chucnang)
        {
            ChucNangModel chucNang = await _dataContext.PhanQuyens.FirstOrDefaultAsync(d => d.MaCN == chucnang.MaCN);
            var exists_role = _dataContext.PhanQuyens.Find(chucnang.MaCN);
            if(chucNang != null)
            {
                _notyfService.Error(" Tên quyền đã có trong database!");
                return View(chucNang);

            }
            exists_role.TenQuyen = chucnang.TenQuyen;
            exists_role.MoTa = chucnang.MoTa;
             _dataContext.Update(exists_role);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Đã cập nhật thành công!");
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int Id)
        {
            ChucNangModel chucNang = await _dataContext.PhanQuyens.FindAsync(Id);
            _dataContext.PhanQuyens.Remove(chucNang);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Đã xoa thành công!");
            return RedirectToAction("Index");

        }
    }
}
