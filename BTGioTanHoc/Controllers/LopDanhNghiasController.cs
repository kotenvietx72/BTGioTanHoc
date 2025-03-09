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
    public class LopDanhNghiasController : Controller
    {
        private Entities db = new Entities();

        // GET: LopDanhNghias
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.LopDanhNghia.ToList());
        }

        // GET: LopDanhNghias/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopDanhNghia lopDanhNghia = db.LopDanhNghia.Find(id);
            if (lopDanhNghia == null)
            {
                return HttpNotFound();
            }
            return View(lopDanhNghia);
        }

        // GET: LopDanhNghias/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LopDanhNghias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "MaLopDN,TenLopDN")] LopDanhNghia lopDanhNghia)
        {
            if (ModelState.IsValid)
            {
                db.LopDanhNghia.Add(lopDanhNghia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lopDanhNghia);
        }

        // GET: LopDanhNghias/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopDanhNghia lopDanhNghia = db.LopDanhNghia.Find(id);
            if (lopDanhNghia == null)
            {
                return HttpNotFound();
            }
            return View(lopDanhNghia);
        }

        // POST: LopDanhNghias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "MaLopDN,TenLopDN")] LopDanhNghia lopDanhNghia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lopDanhNghia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lopDanhNghia);
        }

        // GET: LopDanhNghias/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopDanhNghia lopDanhNghia = db.LopDanhNghia.Find(id);
            if (lopDanhNghia == null)
            {
                return HttpNotFound();
            }
            return View(lopDanhNghia);
        }

        // POST: LopDanhNghias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            LopDanhNghia lopDanhNghia = db.LopDanhNghia.Find(id);
            db.LopDanhNghia.Remove(lopDanhNghia);
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
