using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Laptopshop.Models;

namespace Web_Laptopshop.ViewModel
{
    public class SanPhamVM
    {
        public SanPham SanPham { get; set; }
        public HinhAnhSanPham HinhAnhDaiDien {get;set;}

        public List<HinhAnhSanPham> listHinhAnh { get; set; }
    }
}