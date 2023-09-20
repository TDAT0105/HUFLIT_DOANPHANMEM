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
    public class HoaDonsController : BaseController
    {
        private PTroEntities3 db = new PTroEntities3();


        // GET: Admin/HoaDons
        public ActionResult Index(DateTime? fromDate, DateTime? toDate, string tinhTrangFilter)
        {
            var hoaDons = db.HoaDons.Include(h => h.Customer).Include(h => h.Phong);

            // Lọc hóa đơn theo ngày/tháng/năm
            if (fromDate != null && toDate != null)
            {
                hoaDons = hoaDons.Where(h => h.NgayLapHD >= fromDate && h.NgayLapHD <= toDate);
            }
            // Lọc hóa đơn theo tình trạng
            if (!string.IsNullOrEmpty(tinhTrangFilter) && tinhTrangFilter != "Tất cả")
            {
                hoaDons = hoaDons.Where(h => h.TinhTrang == tinhTrangFilter);
            }
            foreach (var item in hoaDons)
            {
                if (item.Phong != null && item.Phong.Price != null)
                {
                    item.UnitPrice = (double)(item.TienDien + item.TienNuoc + item.TienWifi + item.Phong.Price);
                }
            }
            // Tính tổng tiền các hóa đơn đã thanh toán
            double? totalPaidAmount = hoaDons
    .Where(h => h.TinhTrang == "Đã thanh toán" && h.UnitPrice != null)
    .Sum(h => (double?)h.UnitPrice) ?? 0.0;

            ViewBag.TotalPaidAmount = totalPaidAmount;
            ViewBag.TinhTrangFilter = tinhTrangFilter;
            return View(hoaDons.ToList());
        }

        // GET: Admin/HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
            
        }

        // GET: Admin/HoaDons/Create
        public ActionResult Create(int? idHopDong)
        {
            if (idHopDong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HopDong hopDong = db.HopDongs.Find(idHopDong);
            if (hopDong == null)
            {
                return HttpNotFound();
            }

            // Tạo một hóa đơn mới và điền thông tin từ hợp đồng
            HoaDon newHoaDon = new HoaDon
            {
                CCCD = hopDong.CCCD,
                IDPhong = hopDong.IDPhong,
                NgayLapHD = DateTime.Now, // Ngày lập hóa đơn là ngày thực tế
                TinhTrang = "Chưa thanh toán" // Điền trạng thái hóa đơn
            };

            
            return View(newHoaDon);
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CCCD,IDPhong,NgayLapHD,TienDien,TienNuoc,TienWifi,UnitPrice,TinhTrang")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                // Tính toán trường UnitPrice dựa trên các giá trị được cung cấp
                double unitPrice = (double)hoaDon.TienDien + (double)hoaDon.TienNuoc + (double)hoaDon.TienWifi;

                // Lấy giá thuê phòng (TienPhong) dựa trên IDPhong được cung cấp
                var phong = db.Phongs.Find(hoaDon.IDPhong);
                if (phong != null)
                {
                    unitPrice += (double)phong.Price;
                }

                // Gán giá trị đã tính vào trường UnitPrice
                hoaDon.UnitPrice = unitPrice;

                // Lưu phiếu hóa đơn mới vào cơ sở dữ liệu
                db.HoaDons.Add(hoaDon);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Tạo lại danh sách thả xuống IDPhong nếu mô hình không hợp lệ
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", hoaDon.IDPhong);

            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {   
                return HttpNotFound();
            }
            ViewBag.CCCD = new SelectList(db.Customers, "CCCD", "NameCus", hoaDon.CCCD);
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", hoaDon.IDPhong);
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CCCD,IDPhong,NgayLapHD,TienDien,TienNuoc,TienWifi,UnitPrice,TinhTrang")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CCCD = new SelectList(db.Customers, "CCCD", "NameCus", hoaDon.CCCD);
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", hoaDon.IDPhong);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);

            db.HoaDons.Remove(hoaDon);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pay(int id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            if (hoaDon.TinhTrang == "Chưa thanh toán")
            {
                hoaDon.TinhTrang = "Đã thanh toán";
                db.Entry(hoaDon).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
