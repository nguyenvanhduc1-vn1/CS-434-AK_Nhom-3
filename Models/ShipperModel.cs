using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models
{
    public class ShipperModel
    {
        [Key]
        public int MaShip { get; set; }


		[Required(ErrorMessage = "Yêu cầu nhập tên đăng nhập")]
		public string TenDN { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
		public string SoDienThoai { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]

		public string MatKhau { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập email")]
		public string Email { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }


    }
}
