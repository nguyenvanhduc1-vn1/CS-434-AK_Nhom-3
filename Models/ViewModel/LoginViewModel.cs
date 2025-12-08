using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models.ViewModel
{
    public class LoginViewModel
    {
        [MaxLength(100)]
        [Required(ErrorMessage = " vui lòng nhập địa chỉ email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = " Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [DataType(DataType.Password)] 
        public string MatKhau { get; set; }
    }
}
