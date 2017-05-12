using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using WebDauGia.Helper;
using WebDauGia.Models;

namespace WebDauGia.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }
        // GET: User/Register
        public ActionResult Register()
        {
            RegisterVM model = new RegisterVM()
            {
                Username = "",
                Password = "",
                Name = "",
                Gender = "",
                DateOfBirth = DateTime.Now.ToShortDateString(),
                Email = "",
                Phone = "",
                Address = "",
            };
            return View(model);
        }
        //POST: User/Register
        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("Code", "ExampleCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Captcha validation failed, show error message
                @ViewBag.Error = true;
            }
            else
            {
                // TODO: Captcha validation passed, proceed with protected action
                User u = new User();
                u.UserName = model.Username;
                u.Password = StringUtils.MD5(model.Password);
                u.Name = model.Name;
                u.Gender = model.Gender;
                u.DateOfBirth = DateTime.ParseExact(model.DateOfBirth, "d/M/yyyy", null);
                u.Email = model.Email;
                u.Phone = model.Phone;
                u.Address = model.Address;
                u.DateCreate = DateTime.Now;
                u.AllowAuction = false;
                u.AllowSales = false;
                u.Reliability = "10/10";
                try
                {
                    using (QuanLyDauGiaEntities ctx = new QuanLyDauGiaEntities())
                    {
                        u.DateCreate = DateTime.Now;
                        ctx.Users.Add(u);
                        ctx.SaveChanges();
                    }
                    @ViewBag.Error = false;
                }
                catch (Exception)
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Username đã tồn tại.')</script>");
                }
            }
            return View(model);
        }

        //Get : User/Add
        public ActionResult Add()
        {
            return View();
        }

        //Post : User/Add
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(User model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Added;
                ctx.SaveChanges();
                @ViewBag.Message = "Đã thêm thành công.";
            }
            return View();
        }

        //Get : User/Edit
        public ActionResult Edit(string ID)
        {
            if (ID == "")
            {
                return RedirectToAction("Index", "User");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                User sp = dt.Users
                    .Where(p => p.UserName == ID.ToString())
                    .FirstOrDefault();
                if (sp != null)
                {
                    return View(sp);
                }
                return RedirectToAction("Index", "User");
            }
        }

        //Post: User/Update
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(User model)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ctx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                @ViewBag.Message = "Cập nhật thành công.";
            }
            return RedirectToAction("Edit", "User", new { ID = model.UserName });
        }

        //Get : User/Delete
        public ActionResult Delete(string ID)
        {
            if (ID == "")
            {
                return RedirectToAction("Index", "User");
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                User us = dt.Users
                    .Where(p => p.UserName == ID.ToString())
                    .FirstOrDefault();
                return View(us);
            }
        }

        //Post : User/Deleted
        [HttpPost]
        public ActionResult Deleted(string ID)
        {
            using (var ctx = new QuanLyDauGiaEntities())
            {
                User us = ctx.Users
                    .Where(p => p.UserName == ID)
                    .FirstOrDefault();
                ctx.Entry(us).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }
    }
}