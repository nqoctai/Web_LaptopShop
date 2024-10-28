using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Laptopshop.Models
{
    public class ChiTietGioHang
    {
        public long Id { get; set; }
        public string TenSanPham { get; set; }
        public string AnhBia { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }

        public double ThanhTien
        {
            get { return SoLuong * DonGia; }
        }

        DBConnectDataContext db = new DBConnectDataContext();
        
        public ChiTietGioHang(long MaSp)
        {
            SanPham sp = db.SanPhams.Single(n => n.Id == MaSp);
            if(sp != null)
            {
                Id = MaSp;
                TenSanPham = sp.TenSanPham;
                AnhBia = sp.HinhAnhSanPhams.FirstOrDefault().HinhAnhUrl;
                DonGia = double.Parse(sp.Gia.ToString());
                SoLuong = 1;
            }
        }
    }
}