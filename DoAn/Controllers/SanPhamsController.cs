using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DoAn.Models;
using PagedList;

namespace DoAn.Controllers
{
    public class SanPhamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: SanPhams
        public ActionResult Index(int? page)
        {
            var searchString = Request.Form["timkiem"];
            int pageSize = 16;
            int pageNumber = page ?? 1;
            List<SanPham> sanPhams = db.SanPhams.Include(s => s.HangSP).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = db.SanPhams.Where(sp => sp.tensp.Contains(searchString)).ToList();
            }
            ViewBag.searchString = searchString;
            return View(sanPhams.ToPagedList(pageNumber, pageSize));
            //List<SanPham> sanPhams = db.SanPhams.Include(s => s.HangSP).ToList();
            //int pageSize = 16;
            //int pageNumber = 1;
            //int totalPage = (int)Math.Ceiling((double)sanPhams.Count / pageSize);
            //ViewBag.totalPage = totalPage;
            //List<SanPham> productsOnPage = sanPhams.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            //return View(productsOnPage);
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            List<SanPham> listsp = db.SanPhams.Where(s=>s.mahang == sanPham.mahang).ToList();           
            listsp.Remove(sanPham);
            ViewBag.ds = listsp;
            return View(sanPham);
        }

        // GET: SanPhams/Create
        //public ActionResult Create()
        //{
        //    ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang");
        //    return View();
        //}

        //// POST: SanPhams/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "masp,mahang,tensp,hinhanh,soluong,giaban,mota,CPU,RAM,OS,manhinh,carddohoa,SSD,trangthai")] SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SanPhams.Add(sanPham);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang", sanPham.mahang);
        //    return View(sanPham);
        //}

        //// GET: SanPhams/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SanPham sanPham = db.SanPhams.Find(id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang", sanPham.mahang);
        //    return View(sanPham);
        //}

        //// POST: SanPhams/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "masp,mahang,tensp,hinhanh,soluong,giaban,mota,CPU,RAM,OS,manhinh,carddohoa,SSD,trangthai")] SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(sanPham).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang", sanPham.mahang);
        //    return View(sanPham);
        //}

        //// GET: SanPhams/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SanPham sanPham = db.SanPhams.Find(id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sanPham);
        //}

        //// POST: SanPhams/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SanPham sanPham = db.SanPhams.Find(id);
        //    db.SanPhams.Remove(sanPham);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddCart(int id)
        {
            if (Session["ma"] == null)
            {
                string returnUrl = Url.Action("Index", "SanPhams");
                Session["ReturnUrl"] = returnUrl;
                
                return Json(new { success = false});
            }
            else
            {
                GioHang gh = new GioHang();
                gh.makhachhang = (int)Session["ma"];
                gh.masp = id;
                gh.soluong = 1;
                db.GioHangs.Add(gh);
                db.SaveChanges();
                Session["giohang"] = (int)Session["giohang"] + 1;
                return Json(new { success = true }) ;
            }
        }
        public ActionResult AddCart1(int id)
        {
            if (Session["ma"] == null)
            {
                string returnUrl = Url.Action("Index", "Home");
                Session["ReturnUrl"] = returnUrl;

                return Json(new { success = false });
            }
            else
            {
                GioHang gh = new GioHang();
                gh.makhachhang = (int)Session["ma"];
                gh.masp = id;
                gh.soluong = 1;
                db.GioHangs.Add(gh);
                db.SaveChanges();
                Session["giohang"] = (int)Session["giohang"] + 1;
                return Json(new { success = true });
            }
        }
        public ActionResult AddCart2(int id, int soluong)
        {
            if (Session["ma"] == null)
            {
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues["id"] = id;
                string returnUrl = Url.Action("Details", "SanPhams", routeValues);
                Session["ReturnUrl"] = returnUrl;
                return Json(new { success = false });
            }
            else
            {
                GioHang gh = new GioHang();
                gh.makhachhang = (int)Session["ma"];
                gh.masp = id;
                gh.soluong = soluong;
                db.GioHangs.Add(gh);
                db.SaveChanges();
                Session["giohang"] = (int)Session["giohang"] + 1;
                return Json(new { success = true });
            }
        }

        //public ActionResult TimKiem()
        //{
        //    string tukhoa = Request.Form["timkiem"];
        //    if(tukhoa == null)
        //    {
        //        List<SanPham> listSearch = db.SanPhams.Where(s => s.tensp.Contains(tukhoa)).ToList();
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
