using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models.ViewModel
{
    public class ResetMatKhauViewModel
    {
       
      
            [Required]
            public string Token { get; set; }

            

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string MatKhauMoi { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string XacNhanMatKhauMoi { get; set; }
        }
    }

