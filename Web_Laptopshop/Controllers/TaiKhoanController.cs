using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Laptopshop.Models;
using Web_Laptopshop.ViewModel;

namespace Web_Laptopshop.Controllers
{
    public class TaiKhoanController : Controller
    {
        //
        // GET: /TaiKhoan/
        DBConnectDataContext db = new DBConnectDataContext();

        public ActionResult DangKy()
        {

            return View();
        }

        [HttpPost]
        public ActionResult DangKy(DangKyVM rvm)
        {
            NguoiDung nguoiDung = new NguoiDung();
            nguoiDung.TaiKhoan = rvm.TenNguoiDung;
            nguoiDung.HoTen = rvm.HoTen;
            nguoiDung.SoDienThoai = rvm.SoDienThoai;
            nguoiDung.MatKhau = rvm.MatKhau;
            nguoiDung.Email = rvm.Email;
            db.NguoiDungs.InsertOnSubmit(nguoiDung);
            db.SubmitChanges();
            return View("DangNhap");

        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(DangNhapVM lvm)
        {
            NguoiDung nd = db.NguoiDungs.FirstOrDefault(item => item.TaiKhoan == lvm.TenNguoiDung && item.MatKhau == lvm.MatKhau);
            if (nd != null)
            {
                Session["nd"] = nd;
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

        public ActionResult DangXuat()
        {
            Session["nd"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}
