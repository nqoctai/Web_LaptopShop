using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Laptopshop.Models;

namespace Web_Laptopshop.Areas.QuanLy.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /QuanLy/NguoiDung/

        DBConnectDataContext db = new DBConnectDataContext();
        public ActionResult Index()
        {
            List<NguoiDung> list = db.NguoiDungs.ToList();
            return View(list);
        }

        public ActionResult ThemNguoiDung()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemNguoiDung(FormCollection c, HttpPostedFileBase avatarFile)
        {
          
            var user = new NguoiDung();
            user.TaiKhoan = c["txtTenDangNhap"];
            user.HoTen = c["txtHoVaTen"];
            user.Email = c["txtEmail"];
            user.DiaChi = c["txtDiaChi"];
            user.SoDienThoai = c["txtSDT"];
            user.MatKhau = c["txtMatKhau"];
            user.NgayTao = DateTime.Now;
            if (avatarFile != null && avatarFile.ContentLength > 0)
            {
                // Lấy tên file và đường dẫn lưu trữ
                var fileName = Path.GetFileName(avatarFile.FileName);
                var uploadDir = "~/Content/Upload/Avatar";
                var path = Path.Combine(Server.MapPath(uploadDir), fileName);

                // Lưu file lên server
                avatarFile.SaveAs(path);

                // Gán đường dẫn avatar vào đối tượng người dùng
                user.HinhDaiDien = fileName;
            }
            db.NguoiDungs.InsertOnSubmit(user);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChinhSuaNguoiDung(long id)
        {
            NguoiDung user = db.NguoiDungs.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult ChinhSuaNguoiDung(long id, FormCollection c, HttpPostedFileBase fileUpload)
        {
            var user = db.NguoiDungs.FirstOrDefault(x=>x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Cập nhật thông tin người dùng
            user.TaiKhoan = c["UserName"];
            user.HoTen = c["HoVaTen"];
            user.Email = c["Email"];
            user.DiaChi = c["DiaChi"];
            user.SoDienThoai= c["PhoneNumber"];
            user.MatKhau = c["PasswordHash"];
            // Xử lý file upload
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var uploadDir = "~/Content/Upload/Avatar";
                var path = Path.Combine(Server.MapPath(uploadDir), fileName);

                // Kiểm tra và tạo thư mục upload nếu không tồn tại
                if (!Directory.Exists(Server.MapPath(uploadDir)))
                {
                    Directory.CreateDirectory(Server.MapPath(uploadDir));
                }
                fileUpload.SaveAs(path);
                user.HinhDaiDien = fileName; // Gán đường dẫn avatar vào đối tượng người dùng
            }

            // Cập nhật thông tin người dùng
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XemNguoiDung(long id)
        {
            NguoiDung user = db.NguoiDungs.FirstOrDefault(x=>x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult XoaNguoiDung(long id)
        {
            NguoiDung user = db.NguoiDungs.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult XoaNguoiDung(FormCollection c)
        {

            NguoiDung user = db.NguoiDungs.FirstOrDefault(x => x.Id == double.Parse(c["Id"]));
            if (user == null)
            {
                return HttpNotFound();
            }
            db.NguoiDungs.DeleteOnSubmit(user);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TimKiemTheoEmail(FormCollection c)
        {
            string str_search = c["txtSearch"];
            List<NguoiDung> users = db.NguoiDungs.Where(u => u.Email.Contains(str_search)).ToList();
            return View("Index", users);
        }




    }
}
