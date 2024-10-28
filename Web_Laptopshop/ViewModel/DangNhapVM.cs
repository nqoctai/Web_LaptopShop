using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Laptopshop.ViewModel
{
    public class DangNhapVM
    {
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        public string TenNguoiDung { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; }
    }
}