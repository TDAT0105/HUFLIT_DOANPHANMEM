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
    public class DatLichHen1sController : Controller
    {
        private PTroEntities3 db = new PTroEntities3();

        // GET: DatLichHen1s
        public ActionResult Index()
        {
            var datLichHens = db.DatLichHens.Include(d => d.Phong);
            return View(datLichHens.ToList());
        }

        // GET: DatLichHen1s/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatLichHen datLichHen = db.DatLichHens.Find(id);
            if (datLichHen == null)
            {
                return HttpNotFound();
            }
            return View(datLichHen);
        }

        // GET: DatLichHen1s/Create
        public ActionResult Create(int? id)
        {
            // Lấy danh sách phòng có trạng thái "Trống" và gán cho ViewBag.IDPhong
            var phongsTrong = db.Phongs.Where(p => p.TrangThai == "Trống");
            ViewBag.IDPhong = new SelectList(phongsTrong, "IDPhong", "TenPhong");

            // Tạo đối tượng DatLichHen và tự động điền IDPhong bằng ID phòng đầu tiên trong danh sách có trạng thái "Trống"
            DatLichHen datLichHen = new DatLichHen();
            datLichHen.IDPhong = phongsTrong.FirstOrDefault()?.IDPhong;

            return View(datLichHen);
        }

        // POST: DatLichHen1s/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,HoTen,EmailCus,SDT,IDPhong,NgayHen,GioHen")] DatLichHen datLichHen)
        {
            if (ModelState.IsValid)
            {
                db.DatLichHens.Add(datLichHen);
                db.SaveChanges();
                return RedirectToAction("ListPhong", "Phong1s");
            }

            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", datLichHen.IDPhong);
            return View(datLichHen);
        }

        // GET: DatLichHen1s/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatLichHen datLichHen = db.DatLichHens.Find(id);
            if (datLichHen == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", datLichHen.IDPhong);
            return View(datLichHen);
        }

        // POST: DatLichHen1s/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,HoTen,EmailCus,SDT,IDPhong,NgayHen,GioHen")] DatLichHen datLichHen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datLichHen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", datLichHen.IDPhong);
            return View(datLichHen);
        }

        // GET: DatLichHen1s/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatLichHen datLichHen = db.DatLichHens.Find(id);
            if (datLichHen == null)
            {
                return HttpNotFound();
            }
            return View(datLichHen);
        }

        // POST: DatLichHen1s/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DatLichHen datLichHen = db.DatLichHens.Find(id);
            db.DatLichHens.Remove(datLichHen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ThongBao1()
        {
            return View();
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
