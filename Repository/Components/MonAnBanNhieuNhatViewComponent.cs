using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebDatMonAn.Repository.Components
{
    public class MonAnBanNhieuNhatViewComponent:ViewComponent
    {

        private readonly DataContext _dataContext;
        public MonAnBanNhieuNhatViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var monan = await _dataContext.MonAns.Include(c => c.DanhMuc).OrderByDescending(x => x.NgayTao).Skip(6).Take(6).ToListAsync();
            return View(monan);
        }
    }
}
