using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Laptopshop.Models;
using Web_Laptopshop.ViewModel;

namespace Web_Laptopshop.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        DBConnectDataContext db = new DBConnectDataContext();
        public ActionResult Index()
        {

            var danhSachSanPham = db.SanPhams
                 .Select(sp => new SanPhamVM
                  {
                    SanPham = sp,
                    HinhAnhDaiDien = sp.HinhDaiDien
                    }).ToList();
            List<SanPham> list = db.SanPhams.ToList();
            //HinhAnhSanPham ha = list.FirstOrDefault().HinhAnhSanPhams.FirstOrDefault();
            //ViewBag.ha = ha.HinhAnhUrl;
           
            List<SanPham> listPhuKien = db.SanPhams.Where(x => x.LoaiSanPham.TenLoai != "Laptop").Take(4).ToList();
            var danhSachPhuKien = listPhuKien
                .Select(sp => new SanPhamVM
                {
                    SanPham = sp,
                    HinhAnhDaiDien = sp.HinhDaiDien
                }).ToList();
            ViewBag.DanhSachPhuKien = danhSachPhuKien;
            return View(danhSachSanPham);
        }

        public ActionResult GioHang()
        {
            if (Session["nd"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            GioHang gh = (GioHang)Session["gh"];
            return View(gh);
        }

        public ActionResult ThanhToan()
        {
            return View();
        }

        public ActionResult DatHangThanhCong()
        {
            return View();
        }

        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }


        public ActionResult ThemMatHang(long msp)
        {
            if (Session["nd"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            GioHang gh = (GioHang)Session["gh"];
            if(gh==null)
            {
                gh = new GioHang();
            }

            int kq = gh.Them(msp);
            Session["gh"] = gh;
            return RedirectToAction("Index", "Home");
        }

       

    }
}
