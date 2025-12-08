using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatMonAn.Models
{
    public class HoaDonModel
    {
        [Key]
        public int MaHD { get; set; }
        public DateTime NgayGiao { get; set; }
        public DateTime NgayDat { get; set; }
        public string DiaChi { get; set; }
        public string CachThanhtoan { get; set; }
        public string? loaivanchuyen { get; set; }
        public double? phivanchuyen { get; set; }
        public string? ghichu { get; set; }
        public int TrangThaiDonHang { get; set; }
        public int? TrangThaiGiaoHang { get; set; }
        public string? Toado { get; set; }

        public int MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHangModel KhachHang { get; set; }
        public int MaShip { get; set; }
        [ForeignKey("MaShip")]
        public ShipperModel Shipper { get; set; }
        public int MaNV { get; set; }
        [ForeignKey("MaNV")]
        public NhanVienModel NhanVien { get; set; }


    }
}
