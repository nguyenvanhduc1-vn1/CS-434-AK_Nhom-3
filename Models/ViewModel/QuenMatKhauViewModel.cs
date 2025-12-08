using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models.ViewModel
{
    public class QuenMatKhauViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Nhập chính xác địa chỉ email")]
        public string Email { get; set; }
    }

}
