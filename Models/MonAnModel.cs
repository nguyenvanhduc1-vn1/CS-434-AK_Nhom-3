using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WebDatMonAn.Models
{
    public class MonAnModel
    {
        [Key]
        public int MaMonAn { get; set; }

        [Required, MinLength(4, ErrorMessage = " Yêu cầu nhập tên món ăn")]
        public string TenMonAn { get; set; }

        public string SlugMonAn { get; set; }
        public string HinhAnh { get; set; }

        [Required, MinLength(4, ErrorMessage = " Yêu cầu nhập mô tả món ăn")]
        public string MoTa { get; set; }

        public int TrangThai { get; set; }
        public int SoLuong { get; set; }

        public string DiaChiQuan { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá món ăn")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá món ăn phải lớn hơn 0")]
        public double DonGia { get; set; }

        [Required]
        public string Video { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = " Chọn 1 danh mục")]
        public int MaDanhMuc { get; set; }

        [ForeignKey("MaDanhMuc")]
        public DanhMucModel DanhMuc { get; set; }

        [NotMapped]
        [FileExtensions]
        public IFormFile? ImageUpload { get; set; }

       
    }
}
