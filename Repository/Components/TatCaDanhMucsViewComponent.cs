using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebDatMonAn.Repository.Components
{
    public class TatCaDanhMucsViewComponent:ViewComponent
    {
        private readonly DataContext _dataContext;
        public TatCaDanhMucsViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhMucs = await _dataContext.DanhMucs.Where(d=>d.TrangThai==1).ToListAsync();
            return View(danhMucs);
        }
    }
}
