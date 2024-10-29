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
        //private void AddMoTaSanPham(string linhKienName, string noiDung, SanPham sp)
        //{
        //        var lk = db.LinhKiens.FirstOrDefault(x => x.TenLinhKien == linhKienName);
        //        if (lk != null)
        //        {
        //            var mt = db.MoTas.FirstOrDefault(x => x.NoiDung == noiDung && x.LinhKien == lk);
        //            if (mt == null)
        //            {
        //                mt = new MoTa
        //                {
        //                    LinhKien = lk,
        //                    NoiDung = noiDung
        //                };
        //                db.MoTas.InsertOnSubmit(mt);
        //                // Nếu cần thêm vào MoTas, chèn vào đây
        //            }
        //            var mtsp = new MoTaSanPham
        //            {
        //                MoTa = mt,
        //                SanPham = sp
        //            };
        //            db.MoTaSanPhams.InsertOnSubmit(mtsp);
        //        }
        //}

        private void AddOrUpdateMoTaSanPham(string linhKienName, string noiDung, SanPham sp)
        {
            // Tìm linh kiện dựa trên tên
            var lk = db.LinhKiens.FirstOrDefault(x => x.TenLinhKien == linhKienName);
            if (lk != null)
            {
                // Tìm MoTaSanPham đã tồn tại cho sản phẩm và linh kiện này
                var mtsp = sp.MoTaSanPhams.FirstOrDefault(x => x.MoTa != null && x.MoTa.LinhKien == lk);

                if (mtsp != null)
                {
                    // Kiểm tra xem nội dung có thay đổi không
                    if (mtsp.MoTa.NoiDung != noiDung)
                    {
                        // Tạo mới MoTa với nội dung khác nếu cần
                        var newMoTa = new MoTa
                        {
                            LinhKien = lk,
                            NoiDung = noiDung
                        };
                        db.MoTas.InsertOnSubmit(newMoTa);
                        db.SubmitChanges(); // Lưu để có ID cho MoTa mới

                        // Liên kết MoTaSanPham với MoTa mới
                        mtsp.MoTa = newMoTa;
                    }
                }
                else
                {
                    // Nếu chưa có MoTaSanPham, tạo mới MoTa và MoTaSanPham
                    var newMoTa = new MoTa
                    {
                        LinhKien = lk,
                        NoiDung = noiDung
                    };
                    db.MoTas.InsertOnSubmit(newMoTa);
                    var newMoTaSanPham = new MoTaSanPham
                    {
                        MoTa = newMoTa,
                        SanPham = sp
                    };
                    db.MoTaSanPhams.InsertOnSubmit(newMoTaSanPham);
                }
            }
        }

        [HttpPost]
        public ActionResult ThemSanPham(FormCollection c, IEnumerable<HttpPostedFileBase> hinhanhFile, HttpPostedFileBase hinhdaidienFile)
        {
            SanPham sp = new SanPham();
            sp.TenSanPham = c["txtTen"];
            sp.Gia = decimal.Parse(c["txtGia"]);
            sp.SoLuong = int.Parse(c["txtSoLuong"]);
            sp.TieuDe = c["txtMoTaNgan"];
            string hdhValue = c["txtHDH"];
            string cpuValue = c["txtCPU"];
            string cardValue = c["txtCard"];
            string manHinhValue = c["txtManHinh"];
            string ramValue = c["txtRam"];
            string romValue = c["txtRom"];
            string moTaChiTiet = c["txtMoTaChiTiet"];
            if (moTaChiTiet != null)
            {
                MoTa mt = new MoTa();
                mt.NoiDung = moTaChiTiet;
                MoTaSanPham mtsp = new MoTaSanPham();
                mtsp.MoTa = mt;
                sp.MoTaSanPhams.Add(mtsp);
            }
            if (!string.IsNullOrEmpty(c["txtHDH"]))
            {
                AddOrUpdateMoTaSanPham("HeDieuHanh", hdhValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtCPU"]))
            {
                AddOrUpdateMoTaSanPham("CPU", cpuValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtCard"]))
            {
                AddOrUpdateMoTaSanPham("CardManHinh", cardValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtManHinh"]))
            {
                AddOrUpdateMoTaSanPham("ManHinh", manHinhValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtRam"]))
            {
                AddOrUpdateMoTaSanPham("RAM", ramValue, sp);
            }
            if (!string.IsNullOrEmpty(c["txtRom"]))
            {
                AddOrUpdateMoTaSanPham("ROM", romValue, sp);
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
            if (hinhdaidienFile != null && hinhdaidienFile.ContentLength > 0)
            {
                // Lấy tên file và đường dẫn lưu trữ
                var fileName = Path.GetFileName(hinhdaidienFile.FileName);
                var uploadDir = "~/Content/Upload/HinhAnh";
                var path = Path.Combine(Server.MapPath(uploadDir), fileName);

                // Lưu file lên server
                hinhdaidienFile.SaveAs(path);

                // Gán đường dẫn avatar vào đối tượng người dùng
                sp.HinhDaiDien = fileName;
            }
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
            spVM.HinhAnhDaiDien = sp.HinhDaiDien;
            spVM.listHinhAnh = sp.HinhAnhSanPhams.ToList();
            return View(spVM);
        }

        public ActionResult ChinhSuaSanPham(long? id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.Id == id);
            SanPhamVM spVM = new SanPhamVM();
            spVM.SanPham = sp;
            spVM.HinhAnhDaiDien = sp.HinhDaiDien;
            spVM.listHinhAnh = sp.HinhAnhSanPhams.ToList();
            ViewBag.DanhSachThuongHieu = db.ThuongHieus.ToList();
            ViewBag.DanhSachLoaiSP = db.LoaiSanPhams.ToList();
            ViewBag.DanhSachMucDich = db.PhanKhucs.ToList();
            ViewBag.ListMoTa = sp.MoTaSanPhams.ToList();
            ViewBag.LinhKienFields = new List<LinhKienField>
            {
                    new LinhKienField("HeDieuHanh", "Nhập hệ điều hành", "txtHDH"),
                    new LinhKienField("CPU", "Nhập CPU", "txtCPU"),
                    new LinhKienField("CardManHinh", "Nhập Card màn hình", "txtCard"),
                    new LinhKienField("ManHinh", "Nhập màn hình", "txtManHinh"),
                    new LinhKienField("RAM", "Nhập Ram", "txtRam"),
                    new LinhKienField("ROM", "Nhập Rom", "txtRom")
            };
            return View(spVM);
        }
        [HttpPost]
        public ActionResult ChinhSuaSanPham(long? id, FormCollection c, IEnumerable<HttpPostedFileBase> fileUpload, HttpPostedFileBase hinhdaiDienUpload)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.Id == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            sp.TenSanPham = c["Ten"];
            sp.Gia = decimal.Parse(c["Gia"]);
            sp.SoLuong = int.Parse(c["SoLuong"]);
            sp.TieuDe = c["MoTaNgan"];
      

            // Gán giá trị mặc định nếu biến là null
            string hdhValue = c["txtHDH"] ?? "";
            string cpuValue = c["txtCPU"] ?? "";
            string cardValue = c["txtCard"] ?? "";
            string manHinhValue = c["txtManHinh"] ?? "";
            string ramValue = c["txtRam"] ?? "";
            string romValue = c["txtRom"] ?? "";
            string moTaChiTiet = c["MoTaChiTiet"] ?? "";

            // Gọi hàm AddOrUpdateMoTaSanPham cho từng loại linh kiện
            AddOrUpdateMoTaSanPham("HeDieuHanh", hdhValue, sp);
            AddOrUpdateMoTaSanPham("CPU", cpuValue, sp);
            AddOrUpdateMoTaSanPham("CardManHinh", cardValue, sp);
            AddOrUpdateMoTaSanPham("ManHinh", manHinhValue, sp);
            AddOrUpdateMoTaSanPham("RAM", ramValue, sp);
            AddOrUpdateMoTaSanPham("ROM", romValue, sp);

            var moTaChung = sp.MoTaSanPhams.FirstOrDefault(mtsp => mtsp.MoTa != null && mtsp.MoTa.LinhKien == null);
            if (moTaChung != null)
            {
                moTaChung.MoTa.NoiDung = moTaChiTiet;
            }
            else
            {
                // Nếu chưa có, tạo mới MoTa không có LinhKien
                var moTa = new MoTa
                {
                    NoiDung = moTaChiTiet
                };
                db.MoTas.InsertOnSubmit(moTa);
                db.SubmitChanges();

                var moTaSanPhamChung = new MoTaSanPham
                {
                    MoTa = moTa,
                    SanPham = sp
                };
                db.MoTaSanPhams.InsertOnSubmit(moTaSanPhamChung);
            }
           
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
            if (hinhdaiDienUpload != null && hinhdaiDienUpload.ContentLength > 0)
            {
                // Lấy tên file và đường dẫn lưu trữ
                var fileName = Path.GetFileName(hinhdaiDienUpload.FileName);
                var uploadDir = "~/Content/Upload/HinhAnh";
                var path = Path.Combine(Server.MapPath(uploadDir), fileName);

                // Lưu file lên server
                hinhdaiDienUpload.SaveAs(path);

                // Gán đường dẫn avatar vào đối tượng người dùng
                sp.HinhDaiDien = fileName;
            }
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
