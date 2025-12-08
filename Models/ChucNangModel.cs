using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models
{
    public class ChucNangModel
    {
        [Key]
        public int MaCN { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên chức năng")]
		public string TenQuyen { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập nội dung  ")]
		public string MoTa { get; set; }
        public ICollection<ChiTietChucNang> ChiTietChucNangs { get; set; }
    }
}
