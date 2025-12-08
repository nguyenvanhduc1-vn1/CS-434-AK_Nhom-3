using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models;

namespace WebDatMonAn.Repository
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<DanhMucModel> DanhMucs { get; set; }
        public DbSet<MonAnModel> MonAns { get; set; }
        public DbSet<DanhGiaModel> DanhGias { get; set; }
        public DbSet<KhachHangModel> KhachHangs { get; set; }
        public DbSet<ChiTietChucNang> CTCNs { get; set; }
        public DbSet<DiaDiemModel> DiaDiems { get; set; }
        public DbSet<HoaDonModel> HoaDons { get; set; }
        public DbSet<CTHDModel> CTHDs { get; set; }
        public DbSet<NhanVienModel> NhanViens { get; set; }
        public DbSet<ShipperModel> Shippers { get; set; }
        public DbSet<ChucNangModel> PhanQuyens{ get; set; }
    }
}
