using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using DOAN.Models;


namespace DOAN.Areas.Admin.Controllers
{
    public class HopDongsController : BaseController
    {
        private PTroEntities3 db = new PTroEntities3();

        // GET: Admin/HopDongs
        public ActionResult Index()
        {
           
            
            var check = db.HopDongs.Where(hd => hd.NgayKetThuc == null).ToList();
            foreach (var item in check)
            {
                item.TinhTrang = "Còn hiệu lực";
                item.Customer.TinhTrangThue = "Đang thuê";
                item.Phong.TrangThai = "Đang thuê";

            }
            var check1 = db.HopDongs.Where(hd => hd.NgayKetThuc <= DateTime.Now).ToList();
            foreach (var item in check1)
            {
                item.TinhTrang = "Hết hiệu lực";
                item.Customer.TinhTrangThue = "Đã trả phòng";
                item.Phong.TrangThai = "Trống";
            }

            var check2 = db.HopDongs.Where(hd => hd.NgayKetThuc >= DateTime.Now).ToList();
            foreach (var item in check)
            {
                item.TinhTrang = "Còn hiệu lực";
                item.Customer.TinhTrangThue = "Đang thuê";
                item.Phong.TrangThai = "Đang thuê";

            }
            var check3 = db.HopDongs.Where(hd => hd.NgayBatDau <= hd.NgayKetThuc).ToList();
            foreach (var item in check)
            {
                item.TinhTrang = "Còn hiệu lực";
                item.Customer.TinhTrangThue = "Đang thuê";
                item.Phong.TrangThai = "Đang thuê";

            }
            double? totalReceivedDeposit = db.HopDongs
        .Where(hd => hd.TinhTrang == "Còn hiệu lực")
        .Sum(hd => (double?)hd.TienCoc ?? 0.0);

            double? totalReturnableDeposit = db.HopDongs
        .Where(hd => hd.TinhTrang == "Hết hiệu lực")
        .Sum(hd => (double?)hd.TienCoc ?? 0.0);

            ViewBag.TotalReceivedDeposit = totalReceivedDeposit;
            ViewBag.TotalReturnableDeposit = totalReturnableDeposit;

            var list = db.HopDongs.ToList();
            db.SaveChanges();
            return View(list);

        }

        // GET: Admin/HopDongs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HopDong hopDong = db.HopDongs.Find(id);
            if (hopDong == null)
            {
                return HttpNotFound();
            }
            return View(hopDong);
        }

        // GET: Admin/HopDongs/Create
        public ActionResult Create()
        {
            ViewBag.CCCD = new SelectList(db.Customers, "CCCD", "NameCus");
            var availableRooms = db.Phongs.Where(p => p.TrangThai == "Trống").ToList();
            ViewBag.IDPhong = new SelectList(availableRooms, "IDPhong", "TenPhong");
            HopDong hopDong = new HopDong
            {
                NgayBatDau = DateTime.Now
            };

            return View(hopDong);
        }

        // POST: Admin/HopDongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDHopDong,CCCD,IDPhong,NgayBatDau,NgayKetThuc,TienCoc")] HopDong hopDong)
        {
            if (ModelState.IsValid)
            {
                db.HopDongs.Add(hopDong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CCCD = new SelectList(db.Customers, "CCCD", "NameCus", hopDong.CCCD);
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", hopDong.IDPhong);
            return View(hopDong);
        }

        // GET: Admin/HopDongs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HopDong hopDong = db.HopDongs.Find(id);
            if (hopDong == null)
            {
                return HttpNotFound();
            }
            ViewBag.CCCD = new SelectList(db.Customers, "CCCD", "NameCus", hopDong.CCCD);
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", hopDong.IDPhong);
            return View(hopDong);
        }

        // POST: Admin/HopDongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDHopDong,CCCD,IDPhong,NgayBatDau,NgayKetThuc,TienCoc")] HopDong hopDong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hopDong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CCCD = new SelectList(db.Customers, "CCCD", "NameCus", hopDong.CCCD);
            ViewBag.IDPhong = new SelectList(db.Phongs, "IDPhong", "TenPhong", hopDong.IDPhong);
            return View(hopDong);
        }

        // GET: Admin/HopDongs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HopDong hopDong = db.HopDongs.Find(id);
            if (hopDong == null)
            {
                return HttpNotFound();
            }
            return View(hopDong);
        }

        // POST: Admin/HopDongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HopDong hopDong = db.HopDongs.Find(id);
            db.HopDongs.Remove(hopDong);
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
        public ActionResult EndContract(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HopDong hopDong = db.HopDongs.Find(id);
            if (hopDong == null)
            {
                return HttpNotFound();
            }

            if (hopDong.TinhTrang == "Còn hiệu lực")
            {
                // Cập nhật ngày kết thúc hợp đồng bằng ngày hiện tại
                hopDong.NgayKetThuc = DateTime.Now;
                hopDong.TinhTrang = "Hết hiệu lực";

                // Cập nhật tình trạng của khách hàng
                var khachHangs = db.Customers.Where(kh => kh.CCCD == hopDong.CCCD);
                foreach (var khachHang in khachHangs)
                {
                    khachHang.TinhTrangThue = "Đã trả phòng";
                }

                // Cập nhật tình trạng của phòng
                var phong = db.Phongs.Find(hopDong.IDPhong);
                if (phong != null)
                {
                    phong.TrangThai = "Trống";
                    db.Entry(phong).State = EntityState.Modified;
                }

                db.Entry(hopDong).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public void Check(DateTime ngayNgoaiDoi)
        {
            ngayNgoaiDoi = DateTime.Now;
            using (var db = new PTroEntities3())
            {
                var hopDongs = db.HopDongs.Where(hd => hd.NgayKetThuc < ngayNgoaiDoi).ToList();
                foreach (var hopDong in hopDongs)
                {
                    hopDong.TinhTrang = "Hết Hiệu lực";
                    hopDong.Phong.TrangThai = "Trống";
                    hopDong.Customer.TinhTrangThue = "Đã trả phòng";
                }

                db.SaveChanges();
            }
        }
    }
}
