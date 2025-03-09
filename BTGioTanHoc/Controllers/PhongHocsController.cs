﻿using System;
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
    public class PhongHocsController : Controller
    {
        private Entities db = new Entities();

        // GET: PhongHocs
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.PhongHoc.ToList());
        }

        // GET: PhongHocs/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongHoc phongHoc = db.PhongHoc.Find(id);
            if (phongHoc == null)
            {
                return HttpNotFound();
            }
            return View(phongHoc);
        }

        // GET: PhongHocs/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhongHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "MaPhong,TenPhong,DayNha")] PhongHoc phongHoc)
        {
            if (ModelState.IsValid)
            {
                db.PhongHoc.Add(phongHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phongHoc);
        }

        // GET: PhongHocs/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongHoc phongHoc = db.PhongHoc.Find(id);
            if (phongHoc == null)
            {
                return HttpNotFound();
            }
            return View(phongHoc);
        }

        // POST: PhongHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "MaPhong,TenPhong,DayNha")] PhongHoc phongHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phongHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phongHoc);
        }

        // GET: PhongHocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongHoc phongHoc = db.PhongHoc.Find(id);
            if (phongHoc == null)
            {
                return HttpNotFound();
            }
            return View(phongHoc);
        }

        // POST: PhongHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            PhongHoc phongHoc = db.PhongHoc.Find(id);
            db.PhongHoc.Remove(phongHoc);
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
