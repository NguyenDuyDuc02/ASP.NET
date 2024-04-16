using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class GioHangsController : Controller
    {
        private Model1 db = new Model1();

        // GET: GioHangs
        public ActionResult Index()
        {
            var gioHangs = db.GioHangs.Include(g => g.KhachHang).Include(g => g.SanPham);
            return View(gioHangs.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult RemoveCart(int magiohang)
        {
            GioHang gh = db.GioHangs.Where(s=>s.magiohang == magiohang).FirstOrDefault();
            db.GioHangs.Remove(gh);
            db.SaveChanges();
            Session["giohang"] = (int)Session["giohang"] - 1;
            return Json(new { success = true });
        }

        public ActionResult UpdateCart([Bind(Include = "magiohang,masp,makhachhang,soluong")] GioHang gioHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gioHang).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThanhToan()
        {
            var gioHangs = db.GioHangs.Include(g => g.KhachHang).Include(g => g.SanPham);
            KhachHang taikhoan = db.KhachHangs.Find((int)Session["ma"]);
            ViewBag.taikhoan = taikhoan;
            return View(gioHangs.ToList());
        }

        public ActionResult DatHang()
        {
            KhachHang taikhoan = db.KhachHangs.Find((int)Session["ma"]);
            List<GioHang> dssp = db.GioHangs.Where(s => s.makhachhang == taikhoan.makhachhang).ToList();
            decimal tongtien = 0;
            foreach (var item in dssp)
            {
                tongtien = tongtien + (decimal)(item.SanPham.giaban * item.soluong);
            }
            DonHang donHang = new DonHang();
            donHang.makhachhang = taikhoan.makhachhang;
            donHang.dienthoai = taikhoan.dienthoai;
            donHang.diachi = Request.Form["diachi"] +", " + Request.Form["phuongxa"] + ", " + Request.Form["huyen"] 
                                    + "," + Request.Form["thanhpho"];
            donHang.tongtien = tongtien;
            donHang.ngaydat = DateTime.Now;
            donHang.ngaynhan = donHang.ngaydat.AddDays(3);
            donHang.thanhtoan = "Thanh toán khi nhận hàng";
            db.DonHangs.Add(donHang);
            db.SaveChanges();
            foreach(var item in dssp)
            {
                DonHangChiTiet dhct = new DonHangChiTiet();
                dhct.madonhang = donHang.madonhang;
                dhct.masp = item.masp;
                dhct.soluong = item.soluong;
                dhct.gia = item.SanPham.giaban;
                db.GioHangs.Remove(item);
                db.DonHangChiTiets.Add(dhct);
                db.SaveChanges();
            }
            Session["giohang"] = 0;
            return RedirectToAction("Index", "DonHangs");
        }

        public ActionResult DatHang1()
        {

            List<DonHangChiTiet> dhct = db.DonHangChiTiets.ToList();
            foreach (var item in dhct)
            {
                db.DonHangChiTiets.Remove(item);
            }
            List<DonHang> dh = db.DonHangs.ToList();
            foreach (var item in dh)
            {
                db.DonHangs.Remove(item);
            }
            db.SaveChanges();

            return RedirectToAction("Index", "DonHangs");
        }
    }
}
