using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatMonAn.Models
{
    public class ChiTietChucNang
    {
        [Key]
        public int MaCT { get; set; }
        public int MaCN { get; set; }
        [ForeignKey("MaCN")]
        public ChucNangModel ChucNang { get; set; }
        public int MaNV { get; set; }
        [ForeignKey("MaNV")]
        public NhanVienModel NhanVien { get; set; }
    }
}
