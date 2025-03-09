using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTGioTanHoc.Models;

namespace BTGioTanHoc.Controllers
{ 
    [Authorize]
    public class LopHocPhansController : Controller
    {
        private Entities db = new Entities();

    // GET: LopHocPhans
    [AllowAnonymous]    
        public ActionResult Index()
        {
            var lopHocPhan = db.LopHocPhan.Include(l => l.GiangVien).Include(l => l.LopDanhNghia).Include(l => l.MonHoc);
            return View(lopHocPhan.ToList());
        }

    // GET: LopHocPhans/Details/5
    [AllowAnonymous]    
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHocPhan lopHocPhan = db.LopHocPhan.Find(id);
            if (lopHocPhan == null)
            {
                return HttpNotFound();
            }
            return View(lopHocPhan);
        }

    // GET: LopHocPhans/Create
    [Authorize(Roles = "Administrator")]
    public ActionResult Create()
        {
            ViewBag.MaGiangVien = new SelectList(db.GiangVien, "MaGiangVien", "HoTenGiangVien");
            ViewBag.MaLopDN = new SelectList(db.LopDanhNghia, "MaLopDN", "TenLopDN");
            ViewBag.MaMH = new SelectList(db.MonHoc, "MaMH", "TenMH");
            return View();
        }

        // POST: LopHocPhans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public ActionResult Create([Bind(Include = "MaLHP,MaMH,MaLopDN,SiSo,MaGiangVien")] LopHocPhan lopHocPhan)
        {
            if (ModelState.IsValid)
            {
                db.LopHocPhan.Add(lopHocPhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaGiangVien = new SelectList(db.GiangVien, "MaGiangVien", "HoTenGiangVien", lopHocPhan.MaGiangVien);
            ViewBag.MaLopDN = new SelectList(db.LopDanhNghia, "MaLopDN", "TenLopDN", lopHocPhan.MaLopDN);
            ViewBag.MaMH = new SelectList(db.MonHoc, "MaMH", "TenMH", lopHocPhan.MaMH);
            return View(lopHocPhan);
        }

    // GET: LopHocPhans/Edit/5
    [Authorize(Roles = "Administrator")]
    public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHocPhan lopHocPhan = db.LopHocPhan.Find(id);
            if (lopHocPhan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGiangVien = new SelectList(db.GiangVien, "MaGiangVien", "HoTenGiangVien", lopHocPhan.MaGiangVien);
            ViewBag.MaLopDN = new SelectList(db.LopDanhNghia, "MaLopDN", "TenLopDN", lopHocPhan.MaLopDN);
            ViewBag.MaMH = new SelectList(db.MonHoc, "MaMH", "TenMH", lopHocPhan.MaMH);
            return View(lopHocPhan);
        }

        // POST: LopHocPhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public ActionResult Edit([Bind(Include = "MaLHP,MaMH,MaLopDN,SiSo,MaGiangVien")] LopHocPhan lopHocPhan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lopHocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaGiangVien = new SelectList(db.GiangVien, "MaGiangVien", "HoTenGiangVien", lopHocPhan.MaGiangVien);
            ViewBag.MaLopDN = new SelectList(db.LopDanhNghia, "MaLopDN", "TenLopDN", lopHocPhan.MaLopDN);
            ViewBag.MaMH = new SelectList(db.MonHoc, "MaMH", "TenMH", lopHocPhan.MaMH);
            return View(lopHocPhan);
        }

    // GET: LopHocPhans/Delete/5
    [Authorize(Roles = "Administrator")]
    public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHocPhan lopHocPhan = db.LopHocPhan.Find(id);
            if (lopHocPhan == null)
            {
                return HttpNotFound();
            }
            return View(lopHocPhan);
        }

        // POST: LopHocPhans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public ActionResult DeleteConfirmed(string id)
        {
            LopHocPhan lopHocPhan = db.LopHocPhan.Find(id);
            db.LopHocPhan.Remove(lopHocPhan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
