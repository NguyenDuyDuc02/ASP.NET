using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DoAn.Controllers
{
    public class LoginController : Controller
    {
        Model1 db = new Model1();
        // GET: Login
        public ActionResult Index()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult DangNhap()
        {
            string mail = Request.Form["email"];
            string password = Request.Form["password"];
            var user = db.KhachHangs.Where(s => s.email == mail && s.matkhau == password).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine(password);
                return View("Login");

            }
            else
            {
                Session["ma"] = user.makhachhang;
                Session["hoten"] = user.hoten;
                Session["giohang"] = db.GioHangs.Where(s=>s.makhachhang == user.makhachhang).ToList().Count;
                string returnUrl = Session["ReturnUrl"] as string;
                Session.Remove("ReturnUrl");
                Session.Timeout = 100;
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    // Chuyển hướng đến returnUrl nếu tồn tại
                    return Redirect(returnUrl);  
                }
                else
                {
                    // Chuyển hướng đến trang chính hoặc một trang khác
                    return RedirectToAction("Index", "Home");
                }
                
            }
        }
        //public ActionResult DangNhap(string email, string password)
        //{
        //    var user = db.KhachHangs.FirstOrDefault(u => u.email == email && u.matkhau == password);

        //    if (user == null)
        //    {
        //        ViewBag.error = "Tài khoản hoặc mật khẩu không chính xác !!!";
        //        //return View("Login");
        //        return Json(new { success = false, message = "infor_fail" });
        //    }
        //    else if (user.trangthai == true)
        //    {
        //        //return View("Login");
        //        return Json(new { success = false, message = "account_locked" });
        //    }
        //    else
        //    {
        //        Session["ma"] = user.makhachhang;
        //        Session["hoten"] = user.hoten;
        //        Session["giohang"] = db.GioHangs.Where(s => s.makhachhang == user.makhachhang).ToList().Count;
        //        string returnUrl = Session["ReturnUrl"] as string;
        //        Session.Remove("ReturnUrl");
        //        Session.Timeout = 100;
        //        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        //        {
        //            // Chuyển hướng đến returnUrl nếu tồn tại
        //            //return Redirect(returnUrl);
        //            ViewBag.url = returnUrl;
        //            return Json(new { success = true, message = "has_url" });
        //        }
        //        else
        //        {
        //            // Chuyển hướng đến trang chính hoặc một trang khác
        //            //return RedirectToAction("Index", "Home");
        //            return Json(new { success = true, message = "null_url" });

        //        }

        //    }
        //}

        [HttpPost]
        
        public ActionResult DangKy()
        {
            string hoten = Request.Form["rhoten"];
            string mail = Request.Form["remail"];
            string password = Request.Form["rpassword"];
            string re_password = Request.Form["rrpassword"];
            if(password == re_password)
            {
                KhachHang kh = new KhachHang();
                kh.email = mail;
                kh.matkhau = re_password;
                db.KhachHangs.Add(kh);
            }       
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SignOut()
        {
            Session["ma"] = null;
            Session["hoten"] = null;
            Session["giohang"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}