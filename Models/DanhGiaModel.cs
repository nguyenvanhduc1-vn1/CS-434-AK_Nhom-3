using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatMonAn.Models
{
    public class DanhGiaModel
    {
        [Key]
        public int MaDG { get; set; }
    
   
        public string NoiDung { get; set; }
        public int TrangThai { get; set; }

       
        public int MaHoaDon { get; set; }
        [ForeignKey("MaHoaDon")]
        public HoaDonModel HoaDon { get; set; }
    }
}
