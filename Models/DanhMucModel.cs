using System.ComponentModel.DataAnnotations;

namespace WebDatMonAn.Models
{
    public class DanhMucModel
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên danh mục ")]
        public string TenDanhMuc { get; set; }
        public string SlugDanhMuc { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập nội dung  danh mục")]
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }

}
