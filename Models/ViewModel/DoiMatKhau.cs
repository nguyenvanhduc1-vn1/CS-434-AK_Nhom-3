using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models.ViewModel
{
	public class DoiMatKhau
	{
		[Key]
		public int MaKH { get; set; }
		[Display(Name = "Mật khẩu hiện tại")]
		[ Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
		[MinLength(5,ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
		public string matkhauhientai { get; set; }
		[Display(Name = "Mật khẩu mới")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
		[MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
		public string matkhaumoi { get; set; }
		[Display(Name = "Mật khẩu ")]
		[Required(ErrorMessage = "Vui lòng nhập  lại mật khẩu mới")]
		[MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
		[Compare("matkhaumoi", ErrorMessage = " Mật khẩu không giống nhau!")]
		public string nhaplaimatkhaumoi { get; set; }
	}
}
