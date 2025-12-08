using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebDatMonAn.Models;
using WebDatMonAn.Models.ViewModel;
using WebDatMonAn.Repository;

namespace WebDatMonAn.Controllers
{
	[Authorize]
	public class ThanhToanController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly INotyfService _notyfService;

		public ThanhToanController(DataContext dataContext, INotyfService notyfService)
		{
			_dataContext = dataContext;
			_notyfService = notyfService;
		}
		[HttpGet]
		public IActionResult ThanhToan(string returnUrl = null)
		{
			List<GioHangModel> giohang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
			var taikhoanId = HttpContext.Session.GetString("MaKH");
			MuaHangVM model = new MuaHangVM();
			if (taikhoanId != null)
			{
				var khachhang = _dataContext.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaKH == Convert.ToInt32(taikhoanId));
				model.MaKH = khachhang.MaKH;
				model.TenTK = khachhang.TenTK;
				model.Email = khachhang.Email;
				model.DiaChi = khachhang.DiaChi;
				model.SoDienThoai = khachhang.SoDienThoai;
			}
			ViewData["dsTinhThanh"] = new SelectList(_dataContext.DiaDiems.OrderBy(x => x.MaDD).ToList(), "MaDD", "TenTinhThanh");
			ViewData["dsQuanHuyen"] = new SelectList(_dataContext.DiaDiems.OrderBy(x => x.MaDD).ToList(), "MaDD", "TenQuanHuyen");
			ViewData["dsPhuongXa"] = new SelectList(_dataContext.DiaDiems.OrderBy(x => x.MaDD).ToList(), "MaDD", "TenPhuongXa");
			ViewBag.GioHang = giohang;
			return View(model);
		}
		[HttpPost]
		public IActionResult ThanhToan(MuaHangVM muaHang)
		{
				List<GioHangModel> giohang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
			var taikhoanId = HttpContext.Session.GetString("MaKH");
			MuaHangVM model = new MuaHangVM();
			if (taikhoanId != null)
			{
				var khachhang = _dataContext.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaKH == Convert.ToInt32(taikhoanId));
				model.MaKH = khachhang.MaKH;
				model.TenTK = khachhang.TenTK;
				model.Email = khachhang.Email;
				model.DiaChi = khachhang.DiaChi;
				model.SoDienThoai = khachhang.SoDienThoai;

				khachhang.MaDD = Convert.ToInt32(muaHang.TenTinhThanh);
				

				khachhang.DiaChi = muaHang.DiaChi;
				_dataContext.Update(khachhang);
				_dataContext.SaveChanges();

			}
			try
			{
				if (ModelState.IsValid)
				{
					HoaDonModel hoadon = new HoaDonModel();
					hoadon.MaKH = model.MaKH;
					hoadon.DiaChi = model.DiaChi;
					hoadon.NgayGiao = DateTime.Now;
					hoadon.NgayDat = DateTime.Now;
					hoadon.TrangThaiDonHang = 1;//đơn hàng mới
					hoadon.TrangThaiGiaoHang = 0;//đơn hàng mới
                    hoadon.CachThanhtoan = "chua thanh toan";
					hoadon.phivanchuyen = 15000;
					hoadon.MaNV = 3;
					hoadon.MaShip = 1;
					_dataContext.Add(hoadon);
					_dataContext.SaveChanges();





					foreach (var item in giohang)
					{
						CTHDModel chitiet = new CTHDModel();
						chitiet.MaHD = hoadon.MaHD;
						chitiet.MaMonAn = item.MaMonAn;
						chitiet.soluongban = item.SoLuong;
						chitiet.dongiaban = (float)item.DonGia;


						chitiet.tongtien = (float)item.ThanhTien;

						_dataContext.Add(chitiet);

					}
					_dataContext.SaveChanges();
					HttpContext.Session.Remove("GioHang");
					_notyfService.Success("Đã mua thành công!");

					return RedirectToAction("Bill", "ThanhToan");




				}
			}
			catch (Exception ex)
			{
				return View(model);
			}
		
			return View(model);
		}
		[HttpGet]
        public IActionResult Bill()
        {
            // Lấy mã khách hàng từ session
            var maKHString = HttpContext.Session.GetString("MaKH");

            if (string.IsNullOrEmpty(maKHString))
            {
                return RedirectToAction("Index", "Home"); 
            }

            
            if (!int.TryParse(maKHString, out int maKH))
            {
                return RedirectToAction("Index", "Home"); 
            }

            
            var khachHang = _dataContext.KhachHangs.FirstOrDefault(kh => kh.MaKH == maKH);

            if (khachHang == null)
            {
                return RedirectToAction("Index", "Home");
            }



            var newOrder = _dataContext.HoaDons
                    
                     .OrderByDescending(h => h.NgayDat)
                     .FirstOrDefault();

            if (newOrder != null)
            {

                var orderDetails = _dataContext.CTHDs.Include(ma => ma.MonAn).Include(hd => hd.HoaDon)
                    .Where(od => od.MaHD == newOrder.MaHD)
                    .ToList();

                ViewBag.NewOrder = newOrder;
                ViewBag.OrderDetails = orderDetails;
            }

            return PartialView();
        }


    }

}
