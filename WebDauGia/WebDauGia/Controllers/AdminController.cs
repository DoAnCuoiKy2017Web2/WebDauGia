using System;
using System.Collections;
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

        // GET: Admin/RemoveUser
        public ActionResult RemoveUser()
        {
            return RedirectToAction("ManageUser", "Admin");
        }
        // Post: Admin/RemoveUser
        [HttpPost]
        public ActionResult RemoveUser(string proId)
        {
            try
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
                    }
                }
            }
            catch (Exception)
            {
                TempData["ucheck"] = -1;
            }
            return RedirectToAction("ManageUser", "Admin");
        }
        // GET: Admin/UpdateUserPassword
        public ActionResult UpdateUserPassword()
        {
            return RedirectToAction("ManageUser", "Admin");
        }
        // Post: Admin/UpdateUserPassword
        [HttpPost]
        public ActionResult UpdateUserPassword(string proId, string newpass)
        {
            try
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
                    }
                }
            }
            catch (Exception)
            {
                TempData["ucheck"] = -1;
            }
            return RedirectToAction("ManageUser", "Admin");
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
        // Get: Admin/ManageUser
        public ActionResult ManageUser()
        {
            int t = Convert.ToInt32(TempData["ucheck"]);
            if (t == 1)
            {
                ViewBag.Message = "yes";
            }
            else if (t == -1)
            {
                ViewBag.Message = "no";
            }
            else
            {

            }
            @ViewBag.Active2 = "class=\"active\"";
            return View();
        }

        // Post: Admin/ManageUser
        [HttpPost]
        public ActionResult ManageUser(string proId)
        {
            return View();
        }

        // Get: Admin/ManageCategory
        public ActionResult ManageCategory()
        {
            int t = Convert.ToInt32(TempData["ccheck"]);
            if (t == 1)
            {
                ViewBag.Message = "yes";
            }
            else if (t == -1)
            {
                ViewBag.Message = "no";
            }
            else
            {

            }
            @ViewBag.Active3 = "class=\"active\"";
            return View();
        }
        // Post: Admin/ManageCategory
        [HttpPost]
        public ActionResult ManageCategory(string proId)
        {
            return View();
        }

        // GET: Admin/AddCat
        public ActionResult AddCat()
        {
            return RedirectToAction("ManageCategory", "Admin");
        }
        // Post: Admin/AddCat
        [HttpPost]
        public ActionResult AddCat(string proId)
        {
            try
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
                    }
                }
            }
            catch (Exception)
            {
                TempData["ccheck"] = -1;
            }
            return RedirectToAction("ManageCategory", "Admin");
        }
        // Get: Admin/RemoveCat
        public ActionResult RemoveCat()
        {
            return RedirectToAction("ManageCategory", "Admin");
        }
        // Post: Admin/RemoveCat
        [HttpPost]
        public ActionResult RemoveCat(string proId)
        {
            try
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
                    }
                }
            }
            catch (Exception)
            {
                TempData["ccheck"] = -1;
            }
            return RedirectToAction("ManageCategory", "Admin");

        }

        // GET: Admin/UpdateCat
        public ActionResult UpdateCat()
        {
            return RedirectToAction("ManageCategory", "Admin");
        }
        // Post: Admin/UpdateCat
        [HttpPost]
        public ActionResult UpdateCat(string proId, string newpass)
        {
            try
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
                    }
                }
            }
            catch (Exception)
            {
                TempData["ccheck"] = -1;
            }
            return RedirectToAction("ManageCategory", "Admin");
        }

        // GET: Admin/ManageRequest
        public ActionResult ManageRequest()
        {
            if (TempData["rcheck"] != null)
            {
                int x = Convert.ToInt32(TempData["rcheck"]);
                if (x == 1)
                {
                    ViewBag.Message = "yes";
                }
                else if (x == -1)
                {
                    ViewBag.Message = "no";
                }
                else
                {

                }
            }

            @ViewBag.Active4 = "class=\"active\"";
            return View();
        }

        // GET: Admin/DenyRequest
        public ActionResult DenyRequest()
        {
            return RedirectToAction("ManageRequest", "Admin");
        }
        // Post: Admin/DenyRequest
        [HttpPost]
        public ActionResult DenyRequest(string proId)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                try
                {
                    Request us = dt.Requests
                                       .Where(p => p.UserName == proId)
                                       .FirstOrDefault();
                    if (us != null)
                    {
                        dt.Entry(us).State = System.Data.Entity.EntityState.Deleted;
                        dt.SaveChanges();
                        TempData["rcheck"] = 1;
                        return RedirectToAction("ManageRequest", "Admin");
                    }
                }
                catch (Exception)
                {
                    TempData["rcheck"] = -1;
                }
                return RedirectToAction("ManageRequest", "Admin");
            }
        }

        // GET: Admin/AcceptRequest
        public ActionResult AcceptRequest()
        {
            return RedirectToAction("ManageRequest", "Admin");
        }
        // Post: Admin/AcceptRequest
        [HttpPost]
        public ActionResult AcceptRequest(string proId, string newpass)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                try
                {
                    Request us = dt.Requests
                    .Where(p => p.UserName == proId && p.Request1 == newpass)
                    .FirstOrDefault();
                    if (us != null)
                    {
                        User u = dt.Users
                                .Where(t => t.UserName == us.UserName)
                                .FirstOrDefault();
                        if (newpass.Contains("buy"))
                        {
                            u.AllowAuction = true;
                        }
                        else
                        {
                            u.AllowSales = true;
                        }
                        try
                        {
                            dt.Entry(u).State = System.Data.Entity.EntityState.Modified;
                            dt.SaveChanges();

                            dt.Entry(us).State = System.Data.Entity.EntityState.Deleted;
                            dt.SaveChanges();

                            TempData["rcheck"] = 1;
                        }
                        catch (Exception)
                        {
                            TempData["rcheck"] = -1;
                        }
                    }
                }
                catch (Exception)
                {
                    TempData["rcheck"] = -1;
                }
                return RedirectToAction("ManageRequest", "Admin");
            }
        }
    }
}