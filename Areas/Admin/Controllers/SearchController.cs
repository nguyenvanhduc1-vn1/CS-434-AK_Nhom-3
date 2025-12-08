using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;
using WebDatMonAn.Repository;

namespace WebDatMonAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]


    public class SearchController : Controller
    {
        private readonly DataContext _dataContext;
        public SearchController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpPost]
        public IActionResult TimMonAn(string keyword)
        {
            List<MonAnModel> monan = new List<MonAnModel>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("Index", monan); 
            }

            monan = _dataContext.MonAns.AsNoTracking()
                .Include(c => c.DanhMuc)
                .Where(x => x.TenMonAn.Contains(keyword))
                .Where(x=>x.DanhMuc.TenDanhMuc.Contains(keyword))
                .OrderByDescending(x => x.TenMonAn)
                .Take(4)
                .ToList();

            return PartialView("Index", monan); 
        }
        [HttpPost]
        public IActionResult TimKH (string keyword)
        {
            List<KhachHangModel> khachang = new List<KhachHangModel>();
            if(string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("khachhang", khachang);
            }
            var dsmonan = _dataContext.KhachHangs.AsNoTracking()
                        .Include(c => c.DiaDiem)
                        .Where(x => x.TenTK.Contains(keyword))
                        .Where(x => x.DiaChi.Contains(keyword))
                        .Where(x => x.DiaDiem.TenTinhThanh.Contains(keyword))
                        .Where(x => x.DiaDiem.TenQuanHuyen.Contains(keyword))
                        .Where(x => x.DiaDiem.TenPhuongXa.Contains(keyword))
                        .ToList();
        
            return PartialView("khachhang", khachang);
        }
    }
}
