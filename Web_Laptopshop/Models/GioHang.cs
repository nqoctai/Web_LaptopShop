using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Laptopshop.Models
{
    public class GioHang
    {
        public List<ChiTietGioHang> lst;

        public GioHang()
        {
            lst = new List<ChiTietGioHang>();
        }

        public GioHang(List<ChiTietGioHang> lstGH)
        {
            lst = lstGH;
        }

        public int SoMatHang()
        {
            return lst.Count;
        }

        public int TongSoHang()
        {
            return lst.Sum(x => x.SoLuong);
        }

        public int Them(long MaSp)
        {
            ChiTietGioHang ctgh = lst.Find(n => n.Id == MaSp);
            if(ctgh == null)
            {
                ChiTietGioHang sanpham = new ChiTietGioHang(MaSp);
                if(sanpham == null)
                {
                    return -1;
                }
                lst.Add(sanpham);
            }else
            {
                ctgh.SoLuong++;
            }
            return 1;
        }
    }
}