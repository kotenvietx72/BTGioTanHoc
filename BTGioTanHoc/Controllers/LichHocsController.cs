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
    public class LichHocsController : Controller
    {
        private Entities db = new Entities();

        // GET: LichHocs
        [AllowAnonymous]
        public ActionResult Index()
        {
            var lichHoc = db.LichHoc.Include(l => l.LopHocPhan).Include(l => l.PhongHoc);
            return View(lichHoc.ToList());
        }

        // GET: LichHocs/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichHoc lichHoc = db.LichHoc.Find(id);
            if (lichHoc == null)
            {
                return HttpNotFound();
            }
            return View(lichHoc);
        }

        // GET: LichHocs/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.MaLHP = new SelectList(db.LopHocPhan, "MaLHP", "MaLHP");
            ViewBag.MaPhong = new SelectList(db.PhongHoc, "MaPhong", "TenPhong");
            return View();
        }

        // POST: LichHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "MaLichHoc,MaLHP,MaPhong,NgayHoc,TietBatDau,TietKetThuc,GioTanHoc")] LichHoc lichHoc)
        {
            if (ModelState.IsValid)
            {
                db.LichHoc.Add(lichHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLHP = new SelectList(db.LopHocPhan, "MaLHP", "MaLHP", lichHoc.MaLHP);
            ViewBag.MaPhong = new SelectList(db.PhongHoc, "MaPhong", "TenPhong", lichHoc.MaPhong);
            return View(lichHoc);
        }

        // GET: LichHocs/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichHoc lichHoc = db.LichHoc.Find(id);
            if (lichHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLHP = new SelectList(db.LopHocPhan, "MaLHP", "MaLHP", lichHoc.MaLHP);
            ViewBag.MaPhong = new SelectList(db.PhongHoc, "MaPhong", "TenPhong", lichHoc.MaPhong);
            return View(lichHoc);
        }

        // POST: LichHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "MaLichHoc,MaLHP,MaPhong,NgayHoc,TietBatDau,TietKetThuc,GioTanHoc")] LichHoc lichHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLHP = new SelectList(db.LopHocPhan, "MaLHP", "MaLHP", lichHoc.MaLHP);
            ViewBag.MaPhong = new SelectList(db.PhongHoc, "MaPhong", "TenPhong", lichHoc.MaPhong);
            return View(lichHoc);
        }

        // GET: LichHocs/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichHoc lichHoc = db.LichHoc.Find(id);
            if (lichHoc == null)
            {
                return HttpNotFound();
            }
            return View(lichHoc);
        }

        // POST: LichHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            LichHoc lichHoc = db.LichHoc.Find(id);
            db.LichHoc.Remove(lichHoc);
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
