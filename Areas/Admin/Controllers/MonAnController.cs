using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;
using X.PagedList;

namespace WebDatMonAn.Area.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class MonAnController : Controller
    {
        private readonly DataContext _dataContext;
		private readonly INotyfService _notyfService;
		private readonly IWebHostEnvironment _webHostEnviorment;
        public MonAnController(DataContext dataContext, IWebHostEnvironment webHostEnviorment, INotyfService notyfService)
        {
            _dataContext = dataContext;
            _webHostEnviorment = webHostEnviorment;
            _notyfService = notyfService;
        }
        public IActionResult Index(int page = 1, int CateID = 0)
        {
            int pageSize = 4;
            int pageNumber = page;
            List<MonAnModel> dsmonan = new List<MonAnModel>();

            if (CateID != 0)
            {
                dsmonan = _dataContext.MonAns.AsNoTracking()
                    .Where(x => x.MaDanhMuc == CateID)
                    .Include(x => x.DanhMuc)
                    .OrderByDescending(x => x.MaMonAn)
                    .ToList();
            }
            else
            {
                dsmonan = _dataContext.MonAns.AsNoTracking()
                    .Include(x => x.DanhMuc)
                    .OrderByDescending(x => x.MaMonAn)
                    .ToList();
            }

            PagedList<MonAnModel> models = new PagedList<MonAnModel>(dsmonan.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CateID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", CateID);

            return View(models);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonAnModel monan)
        {
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", monan.MaDanhMuc);
          
                monan.SlugMonAn = monan.TenMonAn.Replace(" ", "-");
                var ten = await _dataContext.MonAns.FirstOrDefaultAsync(p => p.TenMonAn == monan.TenMonAn);
                if (ten != null)
                {
                _notyfService.Error("Món ăn đã có trong database");
                    return View();
                }

                if (monan.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/monan");
            
                    string imagename = Guid.NewGuid().ToString() + "_" + monan.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imagename);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await monan.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    monan.HinhAnh = imagename;

                }
                     monan.NgayTao = DateTime.Now;
           

                _dataContext.Add(monan);
                await _dataContext.SaveChangesAsync();
            _notyfService.Success("Thêm món ăn thành công!");
                return RedirectToAction("Index");

          
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            MonAnModel monAn = await _dataContext.MonAns.FindAsync(Id);
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", monAn.MaDanhMuc);
            return View(monAn);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MonAnModel monan)
        {
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", monan.MaDanhMuc);

     
            if (monan.MaMonAn == null)
            {
                _notyfService.Error("Mã món ăn không hợp lệ!");
                return View(monan);
            }

            var exists_product = await _dataContext.MonAns.FindAsync(monan.MaMonAn);

          
            if (exists_product == null)
            {
                _notyfService.Error("Món ăn không tồn tại!");
                return View(monan);
            }

            monan.SlugMonAn = monan.TenMonAn.Replace(" ", "-");
            var tenmonan = await _dataContext.MonAns.FirstOrDefaultAsync(p => p.TenMonAn == monan.TenMonAn && p.MaMonAn != monan.MaMonAn);

            if (tenmonan != null)
            {
                _notyfService.Error("Món ăn đã tồn tại!");
                return View(monan);
            }

            if (monan.ImageUpload != null)
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/monan");
                string imageName = Guid.NewGuid().ToString() + "_" + monan.ImageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await monan.ImageUpload.CopyToAsync(fs);
                }
                exists_product.HinhAnh = imageName;
            }

            exists_product.TenMonAn = monan.TenMonAn;
            exists_product.MoTa = monan.MoTa;
            exists_product.DiaChiQuan = monan.DiaChiQuan;
            exists_product.SoLuong = monan.SoLuong;
            exists_product.DonGia = monan.DonGia;
            exists_product.Video = monan.Video;
            exists_product.MaDanhMuc = monan.MaDanhMuc;
            exists_product.NgayTao = monan.NgayTao;

            _dataContext.Update(exists_product);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Cập nhật món ăn thành công!");
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int Id)
        {
            MonAnModel monan = await _dataContext.MonAns.FindAsync(Id);
            if (!string.Equals(monan.HinhAnh, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/monan");
                string filePath = Path.Combine(uploadDir, monan.HinhAnh);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _dataContext.MonAns.Remove(monan);
            await _dataContext.SaveChangesAsync();
            _notyfService.Success("Xóa món ăn thành công!");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChiTiet(int Id)
        {
            var monAn = await _dataContext.MonAns.Include(x => x.DanhMuc).FirstOrDefaultAsync(x => x.MaMonAn == Id);
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", monAn.MaDanhMuc);
            return View(monAn);
        }
        [HttpGet]
        public IActionResult Loc(int CateId = 0)
        {
            var url = $"/Admin/MonAn?CateID={CateId}";
            if(CateId == 0)
            {
                url=$"/Admin/MonAn";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
