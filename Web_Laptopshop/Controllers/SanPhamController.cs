using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Laptopshop.Models;
using Web_Laptopshop.ViewModel;

namespace Web_Laptopshop.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/

        DBConnectDataContext db = new DBConnectDataContext();
        public ActionResult Index()
        {
            var danhSachSanPham = db.SanPhams.Where(x => x.LoaiSanPham.TenLoai == "Laptop")
                 .Select(sp => new SanPhamVM
                 {
                     SanPham = sp,
                     HinhAnhDaiDien = sp.HinhDaiDien,
                     listHinhAnh = sp.HinhAnhSanPhams.ToList()
                 }).ToList();
            //List<SanPham> danhSachSanPham = db.SanPhams.Where(x => x.LoaiSanPham.TenLoai == "Laptop").ToList();
            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachPhanKhuc = db.PhanKhucs.ToList();
            return View(danhSachSanPham);
        }

        public ActionResult SanPhamPhuKien()
        {
            var danhSachSanPham = db.SanPhams.Where(x => x.LoaiSanPham.TenLoai != "Laptop")
                 .Select(sp => new SanPhamVM
                 {
                     SanPham = sp,
                     HinhAnhDaiDien = sp.HinhDaiDien
                 }).ToList();
            //List<SanPham> danhSachPhuKien = db.SanPhams.Where(x => x.LoaiSanPham.TenLoai != "Laptop").ToList();
            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachPhanKhuc = db.PhanKhucs.ToList();
            ViewBag.DanhSachLoaiSanPham = db.LoaiSanPhams.Where(x => x.TenLoai != "Laptop").ToList();
            return View(danhSachSanPham);
        }


        [HttpPost]
        public ActionResult LocSanPham(FormCollection c)
        {


            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachPhanKhuc = db.PhanKhucs.ToList();
            ViewBag.DanhSachLoaiSanPham = db.LoaiSanPhams.ToList();


            var thuongHieu = c.GetValues("txtThuongHieu");

            var mucDich = c.GetValues("txtMucDich");

            var gia = c.GetValues("txtMucGia");

            var sapXep = c["txtSapXep"];

            var query = db.SanPhams.AsQueryable();
            query = query.Where(x => x.LoaiSanPham.TenLoai == "Laptop");
            if (thuongHieu != null)
            {
                var longThuongHieuId = thuongHieu.Select(x => long.Parse(x)).ToList();
                query = query.Where(x => longThuongHieuId.Contains(x.ThuongHieuId));
            }

            if (mucDich != null)
            {
                var longMucDichId = mucDich.Select(x => long.Parse(x)).ToList();
                query = query.Where(x => longMucDichId.Contains(x.PhanKhucId));
            }

            //if (gia != null)
            //{
            //   foreach(var mucGia in gia)
            //    {
            //        switch(mucGia)
            //        {
            //            case "duoi-10-trieu":
            //                query = query.Where(x => x.Gia < 10000000);
            //                break;
            //            case "10-15-trieu":
            //                query = query.Where(x => x.Gia >= 10000000 && x.Gia < 15000000);
            //                break;
            //            case "15-20-trieu":
            //                query = query.Where(x => x.Gia >= 15000000 && x.Gia < 20000000);
            //                break;
            //            case "tren-20-trieu":
            //                query = query.Where(x => x.Gia >= 20000000);
            //                break;
            //        }
            //    }
            //}

            if (gia != null)
            {
                query = query.Where(x =>
                    gia.Contains("duoi-10-trieu") && x.Gia < 10000000 ||
                    gia.Contains("10-15-trieu") && x.Gia >= 10000000 && x.Gia < 15000000 ||
                    gia.Contains("15-20-trieu") && x.Gia >= 15000000 && x.Gia < 20000000 ||
                    gia.Contains("tren-20-trieu") && x.Gia >= 20000000
                );
            }

            switch (sapXep)
            {
                case "gia-tang-dan":
                    query = query.OrderBy(x => x.Gia);
                    break;
                case "gia-giam-dan":
                    query = query.OrderByDescending(x => x.Gia);
                    break;
            }
            var danhSachSanPham = query.ToList();

            var dssp = danhSachSanPham
                 .Select(sp => new SanPhamVM
                 {
                     SanPham = sp,
                     HinhAnhDaiDien = sp.HinhDaiDien,
                     listHinhAnh = sp.HinhAnhSanPhams.ToList()
                 }).ToList();
            return View("Index", dssp);
        }


        [HttpPost]
        public ActionResult LocSanPhamPhuKien(FormCollection c)
        {


            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachPhanKhuc = db.PhanKhucs.ToList();
            ViewBag.DanhSachLoaiSanPham = db.LoaiSanPhams.Where(x => x.TenLoai != "Laptop").ToList();

            var loaiSanPham = c.GetValues("txtLoaiSanPham");

            var thuongHieu = c.GetValues("txtThuongHieu");

            var mucDich = c.GetValues("txtPhanKhuc");

            var gia = c.GetValues("txtMucGia");

            var sapXep = c["txtSapXep"];

            var query = db.SanPhams.Where(x => x.LoaiSanPham.TenLoai != "Laptop").AsQueryable();
            if(loaiSanPham != null)
            {
                var longLoaiSanPham = loaiSanPham.Select(x => long.Parse(x)).ToList();
                query = query.Where(x => longLoaiSanPham.Contains(x.LoaiSanPhamId));
            }
            if (thuongHieu != null)
            {
                var longThuongHieuId = thuongHieu.Select(x => long.Parse(x)).ToList();
                query = query.Where(x => longThuongHieuId.Contains(x.ThuongHieuId));
            }

            if (mucDich != null)
            {
                var longMucDichId = mucDich.Select(x => long.Parse(x)).ToList();
                query = query.Where(x => longMucDichId.Contains(x.PhanKhucId));
            }

            //if (gia != null)
            //{
            //   foreach(var mucGia in gia)
            //    {
            //        switch(mucGia)
            //        {
            //            case "duoi-10-trieu":
            //                query = query.Where(x => x.Gia < 10000000);
            //                break;
            //            case "10-15-trieu":
            //                query = query.Where(x => x.Gia >= 10000000 && x.Gia < 15000000);
            //                break;
            //            case "15-20-trieu":
            //                query = query.Where(x => x.Gia >= 15000000 && x.Gia < 20000000);
            //                break;
            //            case "tren-20-trieu":
            //                query = query.Where(x => x.Gia >= 20000000);
            //                break;
            //        }
            //    }
            //}

            if (gia != null)
            {
                query = query.Where(x =>
                    gia.Contains("duoi-10-trieu") && x.Gia < 10000000 ||
                    gia.Contains("10-15-trieu") && x.Gia >= 10000000 && x.Gia < 15000000 ||
                    gia.Contains("15-20-trieu") && x.Gia >= 15000000 && x.Gia < 20000000 ||
                    gia.Contains("tren-20-trieu") && x.Gia >= 20000000
                );
            }

            switch (sapXep)
            {
                case "gia-tang-dan":
                    query = query.OrderBy(x => x.Gia);
                    break;
                case "gia-giam-dan":
                    query = query.OrderByDescending(x => x.Gia);
                    break;
            }
            var danhSachSanPham = query.ToList();
            var dssp = danhSachSanPham
                .Select(sp => new SanPhamVM
                {
                    SanPham = sp,
                    HinhAnhDaiDien = sp.HinhDaiDien,
                    listHinhAnh = sp.HinhAnhSanPhams.ToList()
                }).ToList();
            return View("SanPhamPhuKien", dssp);
        }

        public ActionResult ChiTietSanPham(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.Id == id);
            SanPhamVM spVM = new SanPhamVM();
            spVM.SanPham = sp;
            spVM.HinhAnhDaiDien = sp.HinhDaiDien;
            spVM.listHinhAnh = sp.HinhAnhSanPhams.ToList();
            List<MoTaSanPham> listMoTa = sp.MoTaSanPhams.ToList();
            ViewBag.ListMoTa = listMoTa;
            List<ThuongHieu> listThuongHieu = db.ThuongHieus.ToList();
            ViewBag.ListThuongHieu = listThuongHieu;
            return View(spVM);
        }

    }
}
