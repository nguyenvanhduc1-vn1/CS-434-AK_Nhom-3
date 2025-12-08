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

    public class DiaDiemController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;
        public DiaDiemController(DataContext dataContext, INotyfService notyfService)
        {
            _notyfService = notyfService;
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            var diadiem = _dataContext.DiaDiems.OrderBy(x => x.MaDD).ToList();
            return View(diadiem);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiaDiemModel diaDiem)
        {
           
                var tendiaiem = await _dataContext.DiaDiems.FirstOrDefaultAsync(p => p.TenQuanHuyen == diaDiem.TenQuanHuyen);
                if (tendiaiem != null)
                {
                    _notyfService.Error("tên quận huyện đã có trong database");
                    return View(tendiaiem);
                }
                _dataContext.Add(tendiaiem);
                await _dataContext.SaveChangesAsync();
                _notyfService.Success("Thêm mới địa điểm  thành công!");
                return RedirectToAction("Index");

            
        }
    }
}
