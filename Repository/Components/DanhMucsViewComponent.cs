using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebDatMonAn.Repository.Components
{
    public class DanhMucsViewComponent:ViewComponent
    {
        private readonly DataContext _dataContext;
        public DanhMucsViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhMucs = await _dataContext.DanhMucs.Take(3).ToListAsync();
            
            
            return View(danhMucs);
        }
    }
}
