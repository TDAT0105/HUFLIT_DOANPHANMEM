using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Areas.Admin.Controllers
{
    public class ThanhToansController : Controller
    {
        private PTroEntities3 db = new PTroEntities3();

        // GET: Admin/ThanhToans
        public ActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            var thanhToans = db.ThanhToans.Include(t => t.HoaDon);

            // Lọc thanh toán theo ngày thanh toán
            if (fromDate != null && toDate != null)
            {
                thanhToans = thanhToans.Where(t => t.NgayThanhToan >= fromDate && t.NgayThanhToan <= toDate);
            }

            return View(thanhToans.ToList());
        }

        // GET: Admin/ThanhToans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            return View(thanhToan);
        }

        // GET: Admin/ThanhToans/Create
        public ActionResult Create()
        {
            ViewBag.IDHoaDon = new SelectList(db.HoaDons, "ID", "ID");
            return View();
        }

        // POST: Admin/ThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDThanhToan,IDHoaDon,NgayThanhToan")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                db.ThanhToans.Add(thanhToan);
                db.SaveChanges();

                // Kiểm tra tồn tại của thanh toán
                bool exists = db.ThanhToans.Any(t => t.IDHoaDon == thanhToan.IDHoaDon);
                HoaDon hoaDon = db.HoaDons.FirstOrDefault(h => h.ID == thanhToan.IDHoaDon);

                // Cập nhật trạng thái của hóa đơn
                if (hoaDon != null)
                {
                    if (exists)
                    {
                        hoaDon.TinhTrang = "Đã thanh toán";
                    }
                    else
                    {
                        hoaDon.TinhTrang = "Chưa thanh toán";
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.IDHoaDon = new SelectList(db.HoaDons, "ID", "ID", thanhToan.IDHoaDon);
            return View(thanhToan);
        }

        // GET: Admin/ThanhToans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDHoaDon = new SelectList(db.HoaDons, "ID", "ID", thanhToan.IDHoaDon);
            return View(thanhToan);
        }

        // POST: Admin/ThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDThanhToan,IDHoaDon,NgayThanhToan")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thanhToan).State = EntityState.Modified;
                db.SaveChanges();

                // Đổi trạng thái của hóa đơn thành "Đã thanh toán"
                ThanhToan thanhToanDB = db.ThanhToans.FirstOrDefault(t => t.IDHoaDon == thanhToan.IDHoaDon);
                if (thanhToanDB != null)
                {
                    HoaDon hoaDon = db.HoaDons.FirstOrDefault(h => h.ID == thanhToan.IDHoaDon);
                    if (hoaDon != null)
                    {
                        hoaDon.TinhTrang = "Đã thanh toán";
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.IDHoaDon = new SelectList(db.HoaDons, "ID", "ID", thanhToan.IDHoaDon);
            return View(thanhToan);
        }

        // GET: Admin/ThanhToans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            return View(thanhToan);
        }

        // POST: Admin/ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            db.ThanhToans.Remove(thanhToan);
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
