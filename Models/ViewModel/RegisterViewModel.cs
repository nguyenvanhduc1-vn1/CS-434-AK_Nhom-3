using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models.ViewModel
{
    public class RegisterViewModel
    {

     

      
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string TenTk { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidationEmail", controller: "TaiKhoan")]
        public string Email { get; set; }
        
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số  điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidationPhone", controller: "TaiKhoan")]
        public string  SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string MatKhau { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [Compare("MatKhau", ErrorMessage = "Vui lòng nhập mật khẩu giống nhau")]
        public string NhapMatKhauLai { get; set; }
    }
}
