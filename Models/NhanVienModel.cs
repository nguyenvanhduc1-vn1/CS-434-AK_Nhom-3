using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models
{
    public class NhanVienModel
    {
        [Key]
        public int MaNV { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên nhân viên")]
		public string TenNV { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập mật khẩu truy cập")]
		public string MatKhau { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập địa chỉ nhân  viên")]
		public string DiaChi { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập điện thoại")]
		public string DienThoai { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập email!")]
		public string Email { get; set; }


        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        public ICollection<ChiTietChucNang> ChiTietChucNangs { get; set; }
    }
}
