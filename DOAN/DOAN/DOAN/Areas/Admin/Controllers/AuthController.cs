using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;


namespace DOAN.Areas.Admin.Controllers
{


    public class AuthController : Controller
    {
        PTroEntities3 db = new PTroEntities3();
        // GET: Admin/Auth



        /*[HttpGet]*/
        public ActionResult Login()
        {
            if(!Session["Admin"].Equals(""))
            {
                return RedirectToAction("index", "homeadmin");
            }
            ViewBag.Error = "";
            return View();
        }

        //nơi nhận giá trị
        [HttpPost]
        public ActionResult Login(AdminUser acc, FormCollection form)
        {
            // hứng dữ liệu từ form
            acc.NameUser = form["username"];
            acc.PasswordUser = form["password"];

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(acc.NameUser))
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không tồn tại!");
                }
                if(string.IsNullOrEmpty(acc.PasswordUser))
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
                }
                // check dữ liệu trong database và dữ liệu được hứng
                var checkAdmin = db.AdminUsers.FirstOrDefault(s => s.NameUser == acc.NameUser && s.PasswordUser == acc.PasswordUser);
                if(checkAdmin != null)
                {
                    Session["Admin"] = checkAdmin.NameUser;
                    Session["RoleUser"] = checkAdmin.RoleUser;
                    Session["ID"] = checkAdmin.ID;
                    return RedirectToAction("index", "homeadmin");
                    
                }

                // sai dữ liệu sẽ hiện thông báo
                else
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["Admin"] = "";
            Session["RoleUser"] = "";
            Session["ID"] = "";
            return RedirectToAction("ListPhong","Phong1s");
        }
    }
}