using Microsoft.AspNetCore.Mvc;
using WebDatMonAn.Models;
using WebDatMonAn.Models.ViewModel;

namespace WebDatMonAn.Repository.Components
{
    public class GioHangViewComponent:ViewComponent
    {
        private readonly DataContext _dataContext;
        public GioHangViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IViewComponentResult Invoke()
        {
           var giohang= HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
            return View( new GioHangViewModel
            {
				GioHangs = giohang,
				SoLuong =giohang.Sum(p=>p.SoLuong),
                TongTien=giohang.Sum(c=>c.ThanhTien),
            });
        }
    }
}
