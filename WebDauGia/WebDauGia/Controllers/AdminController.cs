using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDauGia.Helper;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            @ViewBag.Active1 = "class=\"active\"";
            return View();
        }

        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                string pass = StringUtils.MD5(model.PassWord);
                User us = dt.Users
                    .Where(p => p.UserName == model.UserName && p.Password == pass)
                    .FirstOrDefault();
                if (us != null)
                {
                    Session["isLogin"] = 1;
                    Session["user"] = us;
                    Session["username"] = us.UserName;

                    Response.Write("<script LANGUAGE='JavaScript' >alert('Đăng nhập thành công.')</script>");
                    return RedirectToAction("Index", "Admin");
                }
                Response.Write("<script LANGUAGE='JavaScript' >alert('Tên đăng nhập hoặc mật khẩu không đúng')</script>");
                return View();
            }
        }

        // GET: Admin/Login
        public ActionResult RemoveUser()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult RemoveUser(string proId)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                User us = dt.Users
                    .Where(p => p.UserName == proId)
                    .FirstOrDefault();
                if (us != null)
                {
                    dt.Entry(us).State = System.Data.Entity.EntityState.Deleted;
                    dt.SaveChanges();
                    TempData["ucheck"] = 1;
                    return RedirectToAction("ManageUser", "Admin");
                }
                TempData["ucheck"] = 0;
                return RedirectToAction("ManageUser", "Admin");
            }
        }
        // GET: Admin/Login
        public ActionResult UpdateUserPassword()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult UpdateUserPassword(string proId, string newpass)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                User us = dt.Users
                    .Where(p => p.UserName == proId)
                    .FirstOrDefault();
                if (us != null)
                {
                    us.Password = StringUtils.MD5(newpass);
                    dt.Entry(us).State = System.Data.Entity.EntityState.Modified;
                    dt.SaveChanges();
                    TempData["ucheck"] = 1;
                    return RedirectToAction("ManageUser", "Admin");
                }
                TempData["ucheck"] = 0;
                return RedirectToAction("ManageUser", "Admin");
            }
        }
        public ActionResult Test()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult Test(string proId)
        {
            return View();
        }

        public ActionResult ManageUser()
        {
            int t = Convert.ToInt32(TempData["ucheck"]);
            if (t == 1)
            {
                ViewBag.Message = "yes";
            }
            else if(t==0)
            {
                ViewBag.Message = "no";
            }
            else
            {
                
            }
            @ViewBag.Active2 = "class=\"active\"";
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult ManageUser(string proId)
        {
            return View();
        }
        public ActionResult ManageCategory()
        {
            int t = Convert.ToInt32(TempData["ccheck"]);
            if (t == 1)
            {
                ViewBag.Message = "yes";
            }
            else if(t==0)
            {
                ViewBag.Message = "no";
            }
            else
            {

            }
            @ViewBag.Active3 = "class=\"active\"";
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult ManageCategory(string proId)
        {
            return View();
        }
        // GET: Admin/Login
        public ActionResult AddCat()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult AddCat(string proId)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                Category us = new Category();
                us.CatName = proId;

                dt.Entry(us).State = System.Data.Entity.EntityState.Added;
                dt.SaveChanges();
                if (dt.SaveChanges() == 0)
                {
                    TempData["ccheck"] = 1;
                    return RedirectToAction("ManageCategory", "Admin");
                }

                TempData["ccheck"] = 0;
                return RedirectToAction("ManageCategory", "Admin");
            }
        }
        public ActionResult RemoveCat()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult RemoveCat(string proId)
        {
            int id = int.Parse(proId);
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                Category us = dt.Categories
                    .Where(p => p.CatID == id)
                    .FirstOrDefault();
                if (us != null)
                {
                    dt.Entry(us).State = System.Data.Entity.EntityState.Deleted;
                    dt.SaveChanges();
                    TempData["ccheck"] = 1;
                    return RedirectToAction("ManageCategory", "Admin");
                }
                TempData["ccheck"] = 0;
                return RedirectToAction("ManageCategory", "Admin");
            }
        }
        // GET: Admin/Login
        public ActionResult UpdateCat()
        {
            return View();
        }
        // Post: Admin/Login
        [HttpPost]
        public ActionResult UpdateCat(string proId, string newpass)
        {
            int id = int.Parse(proId);
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                Category us = dt.Categories
                    .Where(p => p.CatID == id)
                    .FirstOrDefault();
                if (us != null)
                {
                    us.CatName = newpass;
                    dt.Entry(us).State = System.Data.Entity.EntityState.Modified;
                    dt.SaveChanges();
                    TempData["ccheck"] = 1;
                    return RedirectToAction("ManageCategory", "Admin");
                }
                TempData["ccheck"] = 0;
                return RedirectToAction("ManageCategory", "Admin");
            }
        }

        // GET: Admin/Login
        public ActionResult ManageRequest()
        {
            @ViewBag.Active4 = "class=\"active\"";
            return View();
        }

    }
}