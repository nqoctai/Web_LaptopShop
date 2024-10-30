using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Laptopshop.ViewModel
{
    public class DangKyVM
    {
        [Required(ErrorMessage = "Tên Người Dùng không được để trống")]
        public string TenNguoiDung { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và Xác nhận mật khẩu không khớp")]
        public string XacNhanMatKhau { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string SoDienThoai { get; set; }
    }
}