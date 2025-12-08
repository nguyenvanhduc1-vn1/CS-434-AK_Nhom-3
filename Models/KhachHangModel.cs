using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatMonAn.Models
{
    public class KhachHangModel
    {
        [Key]
        public int MaKH { get; set; }
        public string TenTK { get; set; }
        public string MatKhau { get; set; }
        public string? DiaChi { get; set; }
        public string Email { get; set; }
        public DateTime NgaySinh { get; set; }
        public DateTime NgayTao { get; set; }
        public string SoDienThoai { get; set; }
        public string? Hinh { get; set; }
        public int TrangThai { get; set; }
       
        public int MaDD { get; set; }
        [ForeignKey("MaDD")]
        public DiaDiemModel DiaDiem { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

    }
}
