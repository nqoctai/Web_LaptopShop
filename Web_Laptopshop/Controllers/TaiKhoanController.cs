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
            if(ModelState.IsValid)
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
            return View(rvm);
            

        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(DangNhapVM lvm)
        {
            if(ModelState.IsValid)
            {
                NguoiDung nd = db.NguoiDungs.FirstOrDefault(item => item.TaiKhoan == lvm.TenNguoiDung && item.MatKhau == lvm.MatKhau);
                if (nd != null)
                {
                    Session["nd"] = nd;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    QuanTriVien qtv = db.QuanTriViens.FirstOrDefault(item => item.TaiKhoan == lvm.TenNguoiDung && item.MatKhau == lvm.MatKhau);
                    if (qtv != null)
                    {
                        Session["qtv"] = qtv;
                        return RedirectToAction("Index", "TrangChu", new { area = "QuanLy" });
                    }

                }
            }
            ModelState.AddModelError("MatKhau", "Tên người dùng hoặc mật khẩu không đúng");
            return View(lvm);
            
        }

        public ActionResult DangXuat()
        {
            Session["nd"] = null;
            Session["gh"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}
