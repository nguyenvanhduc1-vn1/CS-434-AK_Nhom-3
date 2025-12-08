namespace WebDatMonAn.Models
{
    public class GioHangModel
    {
        public int MaMonAn { get; set; }
        public string HinhAnh { get; set; }
        public string TenMonAn { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get { return SoLuong * DonGia; } }
        public GioHangModel()
        {

        }
        public GioHangModel(MonAnModel Monan)
        {
            MaMonAn = Monan.MaMonAn;
            TenMonAn = Monan.TenMonAn;
            DonGia = Monan.DonGia;
            HinhAnh = Monan.HinhAnh;
        }
    }

}
