using CMS.Models;
using CMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CMS.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                base.ViewBag.Title = "Trang chủ";
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult BangGia()
        {
            return View();
        }
        public ActionResult LienHe(int? trangThai = null)
        {
            ViewBag.mgs = trangThai;
            return View();
        }
        [HttpPost]
        public ActionResult Dangky()
        {
            NguoiDung nguoiDung = new NguoiDung();
            nguoiDung.TenNguoiDung = Request.Form["tenkhachhang"];
            nguoiDung.DiaChi = Request.Form["diachi"];
            nguoiDung.SoDienThoai = Request.Form["sodienthoai"];
            nguoiDung.SoKhac = Request.Form["sodienthoaikhac"];
            nguoiDung.Gmail = Request.Form["email"];
            NguoiDungService nguoiDungService = new NguoiDungService();
            bool result = nguoiDungService.ThemNguoiDung(nguoiDung);
            if (result)
            {
                return RedirectToAction("LienHe", new { trangThai = 4 });
            }
            else return View();            
        }
        [HttpPost]
        public ActionResult DatLich()
        {
            ViewBag.message = null;

            NguoiDung nguoiDung = new NguoiDung();
            nguoiDung.SoDienThoai = Request.Form["sodienthoai"];

            KhachDatHang khachDatHang = new KhachDatHang();
            khachDatHang.NgayDat = Convert.ToDateTime(Request.Form["ngaydat"]);
            khachDatHang.GioDat = int.Parse(Request.Form["giodat"]);
            int gioDat = int.Parse(Request.Form["giodat"]);

            KhachDatHangService khachDatHangService = new KhachDatHangService();
            int result = khachDatHangService.ThemKhachDatHang(khachDatHang, nguoiDung);
            if (result == 2)
            {
                ViewBag.message = "Đặt lịch thành công!";
                return RedirectToAction("LienHe", new { trangThai = result });
            }
            else if (result == 1)
            {
                ViewBag.message = "Đặt lịch thành công!";
                return RedirectToAction("LienHe", new { trangThai = result });
            }
            else
            {
                ViewBag.message = "Có lỗi xảy ra, vui lòng thử lại!";
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult ThemLienHe()
        {
            LienHe lienHe = new LienHe();
            lienHe.NoiDung = Request.Form["noidung"];
            lienHe.HoTen = Request.Form["tennguoidung"];
            lienHe.SoDienThoai = Request.Form["sodienthoai"];

            LienHeService lienHeService = new LienHeService();
            lienHeService.ThemLienHe(lienHe);
            return RedirectToAction("Index", "LienHe");
        }
        public ActionResult NguoiDungChuaTonTai()
        {
            return View();
        }
        public ActionResult DatLichThatBai()
        {
            return View();
        }
    }
}
