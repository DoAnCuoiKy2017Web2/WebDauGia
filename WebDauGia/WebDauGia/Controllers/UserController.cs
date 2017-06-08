using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using WebDauGia.Helper;
using WebDauGia.Models;
using WebDauGia.Filters;

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
            LoginVM model = new LoginVM();
            model.UserName = "";
            return View(model);
        }
        // Post: User/Login
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
                    if(model.Remember != null)
                    {
                        //Cái này xử lí nếu người dùng check Ghi nhớ đăng nhập
                        Response.Cookies["userID"].Value = us.UserName.ToString();
                        Response.Cookies["userID"].Expires = DateTime.Now.AddDays(7);
                    }
                    Session["isLogin"] = 1;
                    Session["user"] = us;
                    Session["username"] = us.UserName;

                    Response.Write("<script LANGUAGE='JavaScript' >alert('Đăng nhập thành công.')</script>");
                    return RedirectToAction("Index", "Home");
                }
                Response.Write("<script LANGUAGE='JavaScript' >alert('Tên đăng nhập hoặc mật khẩu không đúng')</script>");
                return View(model);
            }
        }
        // Post: User/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            CurrentContext.Destroy();
            return RedirectToAction("Index", "Home");
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

                    LoginVM lvm = new LoginVM();
                    lvm.UserName = model.Username;
                    lvm.PassWord = model.Password;
                    Login(lvm);

                    Response.Write("<script LANGUAGE='JavaScript' >alert('Đăng ký thành công. Đang chuyển về trang chủ')</script>");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Username đã tồn tại.')</script>");
                }
            }
            return View(model);
        }
        //Get : User/Edit
        [CheckLogin]
        public ActionResult Edit(string ID)
        {
            if (ID == "" || ID == null)
            {
                if (CurrentContext.IsLogged() == false)
                    return RedirectToAction("Index", "User");
                else
                {
                    ID = CurrentContext.GetCurUser().UserName;
                }
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                User us = dt.Users
                    .Where(p => p.UserName == ID.ToString())
                    .FirstOrDefault();
                if (us != null)
                {
                    return View(us);
                }
                return RedirectToAction("Index", "Home");
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

        //Get : user/manageuser

        public ActionResult ManageUser()
        {
            List<User> listUser = null;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                listUser = ctx.Users.ToList();
            }
            return View(listUser);
        }
        //Get : User/Profile
        [CheckLogin]
        public ActionResult Profile(string ID)
        {
            if (ID == "" || ID == null)
            {
                if (CurrentContext.IsLogged() == false)
                    return RedirectToAction("Index", "User");
                else
                {
                    ID = CurrentContext.GetCurUser().UserName;
                }
            }
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {

                User us = dt.Users
                    .Where(p => p.UserName == ID.ToString())
                    .FirstOrDefault();
                if (us != null)
                {
                    return View(us);
                }
                return RedirectToAction("Index", "Home");
            }
        }
    }
}