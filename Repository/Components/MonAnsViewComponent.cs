using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebDatMonAn.Repository.Components
{
    public class MonAnsViewComponent:ViewComponent
    {
        private readonly DataContext _dataContext;
        public MonAnsViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var monan = await _dataContext.MonAns.Include(c=>c.DanhMuc).OrderByDescending(x=>x.NgayTao).Take(3).ToListAsync();
            return View(monan);
        }
    }
}
