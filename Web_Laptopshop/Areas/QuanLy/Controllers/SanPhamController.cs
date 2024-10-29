using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Laptopshop.Models;
using Web_Laptopshop.ViewModel;

namespace Web_Laptopshop.Areas.QuanLy.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /QuanLy/SanPham/
        DBConnectDataContext db = new DBConnectDataContext();
        public ActionResult Index()
        {
            List<SanPham> danhSachSanPham = db.SanPhams.ToList();
            return View(danhSachSanPham);
        }

        public ActionResult ThemSanPham()
        { 
            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachLoaiSP = db.LoaiSanPhams.ToList();
            ViewBag.DanhSachPhanKhuc = db.PhanKhucs.ToList();
            return View();
        }
        private void AddMoTaSanPham(string linhKienName, string noiDung, SanPham sp)
        {
                var lk = db.LinhKiens.FirstOrDefault(x => x.TenLinhKien == linhKienName);
                if (lk != null)
                {
                    var mt = db.MoTas.FirstOrDefault(x => x.NoiDung == noiDung && x.LinhKien == lk);
                    if (mt == null)
                    {
                        mt = new MoTa
                        {
                            LinhKien = lk,
                            NoiDung = noiDung
                        };
                        db.MoTas.InsertOnSubmit(mt);
                        // Nếu cần thêm vào MoTas, chèn vào đây
                    }
                    var mtsp = new MoTaSanPham
                    {
                        MoTa = mt,
                        SanPham = sp
                    };
                    db.MoTaSanPhams.InsertOnSubmit(mtsp);
                }
        }

        [HttpPost]
        public ActionResult ThemSanPham(FormCollection c, IEnumerable<HttpPostedFileBase> hinhanhFile)
        {
            SanPham sp = new SanPham();
            sp.TenSanPham = c["txtTen"];
            sp.Gia = decimal.Parse(c["txtGia"]);
            sp.SoLuong = int.Parse(c["txtSoLuong"]);
            sp.TieuDe = c["txtMoTaNgan"];
            string cpuValue = c["txtCPU"];
            string cardValue = c["txtCard"];
            string manHinhValue = c["txtManHinh"];
            string ramValue = c["txtRam"];
            string romValue = c["txtRom"];
            if (!string.IsNullOrEmpty(c["txtCPU"]))
            {
                AddMoTaSanPham("CPU", cpuValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtCard"]))
            {
                AddMoTaSanPham("CardManHinh", cardValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtManHinh"]))
            {
                AddMoTaSanPham("ManHinh", manHinhValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtRam"]))
            {
                AddMoTaSanPham("RAM", ramValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtRom"]))
            {
                AddMoTaSanPham("ROM", romValue, sp);
            }
            string idThuongHieu = c["txtThuongHieu"];
            ThuongHieu thuongHieu = db.ThuongHieus.FirstOrDefault(x=> x.Id == long.Parse(idThuongHieu));
            sp.ThuongHieu = thuongHieu;
            string idLoaiSP = c["txtLoaiSanPham"];
            LoaiSanPham loaiSP = db.LoaiSanPhams.FirstOrDefault( x=> x.Id == long.Parse(idLoaiSP));
            sp.LoaiSanPham = loaiSP;
            string idMucDich = c["txtMucDich"];
            PhanKhuc pk = db.PhanKhucs.FirstOrDefault( x => x.Id == long.Parse(idMucDich));
            sp.PhanKhuc = pk;
            if (hinhanhFile != null)
            {
                foreach(var file in hinhanhFile)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploadDir = "~/Content/Upload/HinhAnh";
                    var path = Path.Combine(Server.MapPath(uploadDir), fileName);
                    // Lưu file lên server
                    file.SaveAs(path);
                    var hinhAnh = new HinhAnhSanPham
                    {
                        SanPhamId = sp.Id,
                        HinhAnhUrl = fileName
                    };
                    sp.HinhAnhSanPhams.Add(hinhAnh);
                }
                // Lấy tên file và đường dẫn lưu trữ
               

                

                // Gán đường dẫn avatar vào đối tượng người dùng
                //sp.HinhAnhUrl = fileName;
            }
            db.SanPhams.InsertOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XemSanPham(long? id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.Id == id);
            SanPhamVM spVM = new SanPhamVM();
            spVM.SanPham = sp;
            spVM.HinhAnhDaiDien = sp.HinhAnhSanPhams.FirstOrDefault();
            spVM.listHinhAnh = sp.HinhAnhSanPhams.ToList();
            return View(spVM);
        }

        public ActionResult ChinhSuaSanPham(long? id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.Id == id);
            SanPhamVM spVM = new SanPhamVM();
            spVM.SanPham = sp;
            spVM.HinhAnhDaiDien = sp.HinhAnhSanPhams.FirstOrDefault();
            spVM.listHinhAnh = sp.HinhAnhSanPhams.ToList();
            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachLoaiSP = db.LoaiSanPhams.ToList();
            ViewBag.DanhSachMucDich = db.PhanKhucs.ToList();
            return View(spVM);
        }
        [HttpPost]
        public ActionResult ChinhSuaSanPham(long? id, FormCollection c, IEnumerable<HttpPostedFileBase> fileUpload)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.Id == id);
            sp.TenSanPham = c["Ten"];
            sp.Gia = decimal.Parse(c["Gia"]);
            sp.SoLuong = int.Parse(c["SoLuong"]);
            sp.TieuDe = c["MoTaNgan"];
            string maTH = c["ThuongHieu"];
            ThuongHieu th = db.ThuongHieus.FirstOrDefault(x=>x.Id == long.Parse(maTH));
            sp.ThuongHieu = th;
            string idLoaiSP = c["LoaiSanPham"];
            LoaiSanPham loaiSP = db.LoaiSanPhams.FirstOrDefault(x=> x.Id == long.Parse(idLoaiSP));
            sp.LoaiSanPham = loaiSP;
            string idMucDich = c["MucDich"];
            PhanKhuc mucDich = db.PhanKhucs.FirstOrDefault(x=>x.Id == long.Parse(idMucDich));
            sp.PhanKhuc = mucDich;
            //sp.PhanKhucSanPham = c["PhanKhuc"];
            if (fileUpload != null)
            {
                sp.HinhAnhSanPhams.Clear();
                foreach (var file in fileUpload)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploadDir = "~/Content/Upload/HinhAnh";
                    var path = Path.Combine(Server.MapPath(uploadDir), fileName);
                    // Lưu file lên server
                    file.SaveAs(path);
                    var hinhAnh = new HinhAnhSanPham
                    {
                        SanPhamId = sp.Id,
                        HinhAnhUrl = fileName
                    };
                    sp.HinhAnhSanPhams.Add(hinhAnh);
                }
                // Lấy tên file và đường dẫn lưu trữ
                // Gán đường dẫn avatar vào đối tượng người dùng
                //sp.HinhAnhUrl = fileName;
            }
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult XoaSanPham(long? id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x=>x.Id == id);
            return View(sp);
        }


        [HttpPost]
        public ActionResult XoaSanPham(FormCollection c)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x=>x.Id == long.Parse(c["ID"]));
            List<HinhAnhSanPham> listHA = sp.HinhAnhSanPhams.ToList();
            List<MoTaSanPham> listMota = sp.MoTaSanPhams.ToList();
            db.MoTaSanPhams.DeleteAllOnSubmit(listMota);
            db.HinhAnhSanPhams.DeleteAllOnSubmit(listHA);
            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult TimKiemSanPham(FormCollection c)
        {
            string searchText = c["txtSearch"];
            List<SanPham> danhSachSanPham = db.SanPhams.Where(sp => sp.TenSanPham.Contains(searchText)).ToList();
            return View("Index", danhSachSanPham);
        }


    }
}
