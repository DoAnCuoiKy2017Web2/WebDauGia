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
    }
}