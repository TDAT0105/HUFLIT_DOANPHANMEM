using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    public class Phong1sController : Controller
    {
        private PTroEntities3 db = new PTroEntities3();

        // GET: Phong1s
        public ActionResult Index()
        {
            return View(db.Phongs.ToList());
        }

        // GET: Phong1s/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // GET: Phong1s/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Phong1s/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDPhong,TenPhong,DienTich,Decription,Price,TrangThai,ImagePhong1,ImagePhong2,ImagePhong3")] Phong phong)
        {
            if (ModelState.IsValid)
            {
                db.Phongs.Add(phong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phong);
        }

        // GET: Phong1s/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Phong1s/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDPhong,TenPhong,DienTich,Decription,Price,TrangThai,ImagePhong1,ImagePhong2,ImagePhong3")] Phong phong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phong);
        }

        // GET: Phong1s/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Phong1s/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Phong phong = db.Phongs.Find(id);
            db.Phongs.Remove(phong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ListPhong(decimal? minPrice, decimal? maxPrice, string searchName)
        {
            var phongTrong = db.Phongs.Where(p => p.TrangThai == "Trống");

            // Lọc phòng theo giá
            if (minPrice != null && maxPrice != null)
            {
                phongTrong = phongTrong.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            // Tìm kiếm phòng theo tên
            if (!string.IsNullOrEmpty(searchName))
            {
                phongTrong = phongTrong.Where(p => p.TenPhong.Contains(searchName));
            }

            // Trả về kết quả lọc và tìm kiếm
            return View(phongTrong.ToList());
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