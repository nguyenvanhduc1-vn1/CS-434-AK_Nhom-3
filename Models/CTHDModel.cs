using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatMonAn.Models
{
    public class CTHDModel
    {
        [Key]
        public int MaCT { get; set; }
        public int MaHD { get; set; }
        [ForeignKey("MaHD")]
        public HoaDonModel HoaDon { get; set; }
        public int MaMonAn { get; set; }
        [ForeignKey("MaMonAn")]
        public MonAnModel MonAn { get; set; }
        public int soluongban { get; set; }
        public float dongiaban { get; set; }
        public double tongtien { get; set; }
    }
}
