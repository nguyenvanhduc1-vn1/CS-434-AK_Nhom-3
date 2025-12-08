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
    public class HoaDonController : Controller
    {
        private readonly DataContext _dataContext;
        public HoaDonController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
		[HttpGet]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var order = _dataContext.HoaDons.Include(x => x.KhachHang).AsNoTracking()
                       .OrderByDescending(x => x.NgayDat);
            PagedList<HoaDonModel> models = new PagedList<HoaDonModel>(order, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
		[HttpPost]

		public async Task<IActionResult> CapNhatTrangThai(int id)
		{
			var order = await _dataContext.HoaDons.FirstOrDefaultAsync(x => x.MaHD == id);
			if (order == null)
			{
				return NotFound();
			}

			
			order.TrangThaiDonHang = 0; 

			try
			{
				_dataContext.Update(order);
				await _dataContext.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// Xử lý lỗi
				return StatusCode(500, $"Lỗi xảy ra khi cập nhật trạng thái đơn hàng: {ex.Message}");
			}
		}
        public async Task<IActionResult> ChiTiet(int Id)
        {
            var chiTietHoaDons = await _dataContext.CTHDs.Include(ma => ma.MonAn).Include(x=>x.HoaDon)
                                               .Where(c => c.MaHD == Id)
                                                  .ToListAsync();


            return View(chiTietHoaDons);

        }

    }
}
