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
        // GET: Test
        [CheckLogin]
        public ActionResult Test()
        {
            string ID = CurrentContext.GetCurUser().UserName;
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                User us = dt.Users
                    .Where(p => p.UserName == ID.ToString())
                    .FirstOrDefault();
                if (us != null)
                {
                    return View(us);
                }
            }
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
                    if (model.Remember != null)
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
        //Post: User/Sale
        [CheckLogin]
        [HttpPost]
        public ActionResult Sale(string name)
        {
            try
            {
                using (var ctx = new QuanLyDauGiaEntities())
                {
                    var check = ctx.Requests.Where(r => r.UserName == name && r.Request1 == "sale").FirstOrDefault();
                    //nếu chưa tồn tại  thì add
                    if (check == null)
                    {
                        var rq = new Request();
                        rq.UserName = name;
                        rq.TimeRequest = DateTime.Now;
                        rq.Request1 = "sale";
                        rq.Expire = null;
                        ctx.Requests.Add(rq);
                        ctx.SaveChanges();
                        return Json("Gửi Yêu Cầu Thành Công! Vui Lòng Chờ Phẩn Hồi Sớm Nhất Từ Admin", JsonRequestBehavior.AllowGet);
                    }
                    //neu da ton tai và trong luc duyet thi thong bao
                    if (check.Expire == null)
                    {
                        return Json("Gửi Yêu Cầu Thất Bại! Yêu Của Bạn Đang Chờ Duyệt, Vui Lòng Chờ Phản Hồi Từ Admin", JsonRequestBehavior.AllowGet);
                    }
                    //nếu đã tồn tại nhưng hết hạn thì update
                    check.Expire = null;
                    ctx.Entry(check).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                    return Json("Gửi Yêu Cầu Thành Công! Vui Lòng Chờ Phẩn Hồi Sớm Nhất Từ Admin", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Hiện Tại Không Thể Gửi Yêu Cầu!!!", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: User/Register
        public ActionResult Register()
        {
            RegisterVM model = new RegisterVM()
            {
                UserName = "",
                PassWord = "",
                Name = "",
                Gender = "",
                DateOfBirth = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year,
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
                using (QuanLyDauGiaEntities ctx = new QuanLyDauGiaEntities())
                {
                    User us = ctx.Users.Where(p => p.Email == model.Email.ToString()).FirstOrDefault();
                    if (us != null)
                    {
                        Response.Write("<script LANGUAGE='JavaScript' >alert('Email đã tồn tại.')</script>");
                    }
                    else
                    {
                        // TODO: Captcha validation passed, proceed with protected action
                        User u = new User();
                        u.UserName = model.UserName;
                        u.Password = StringUtils.MD5(model.PassWord);
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
                            u.DateCreate = DateTime.Now;
                            ctx.Users.Add(u);
                            ctx.SaveChanges();

                            @ViewBag.Error = false;

                            LoginVM lvm = new LoginVM();
                            lvm.UserName = model.UserName;
                            lvm.PassWord = model.PassWord;
                            Login(lvm);

                            Response.Write("<script LANGUAGE='JavaScript' >alert('Đăng ký thành công. Đang chuyển về trang chủ')</script>");
                            return RedirectToAction("Index", "Home");
                        }
                        catch (Exception)
                        {
                            Response.Write("<script LANGUAGE='JavaScript' >alert('Username đã tồn tại.')</script>");
                        }
                    }
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

        // GET: User/Update
        [CheckLogin]
        public ActionResult Update()
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
            string ID = CurrentContext.GetCurUser().UserName;

            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                User us = dt.Users
                    .Where(p => p.UserName == ID.ToString())
                    .FirstOrDefault();
                return View(us);
            }
        }
        //Post: User/Update
        [CheckLogin]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(RegisterVM model)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                try
                {
                    User us = dt.Users
               .Where(p => p.UserName == model.UserName)
               .FirstOrDefault();

                    if (us != null)
                    {
                        us.Name = model.Name;
                        us.Address = model.Address;
                        us.Email = model.Email;
                        us.Phone = model.Phone;
                        us.Gender = model.Gender;
                        us.DateOfBirth = DateTime.ParseExact(model.DateOfBirth, "d/M/yyyy", null);
                        using (var ctx = new QuanLyDauGiaEntities())
                        {
                            ctx.Entry(us).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            @ViewBag.Message = "yes";
                            TempData["ucheck"] = 1;
                        }
                    }
                    return RedirectToAction("Update", "User", new { ID = model.UserName });
                }
                catch (Exception)
                {
                    TempData["ucheck"] = -1;
                }
                return RedirectToAction("Update", "User", new { ID = model.UserName });
            }
        }

        //Get : User/Delete
        [CheckLogin]
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
        public ActionResult Profile()
        {
            string ID;
            if (CurrentContext.IsLogged() == false)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                ID = CurrentContext.GetCurUser().UserName;
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

        // GET: User/ChangePass
        [CheckLogin]
        public ActionResult ChangePass()
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
            return View();
        }
        // Post: User/ChangePass

        [HttpPost]
        [CheckLogin]
        public ActionResult ChangePass(string un, string tOPassWord, string tNPassWord)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                try
                {
                    string pass = StringUtils.MD5(tOPassWord);
                    User us = dt.Users
                        .Where(p => p.UserName == un && p.Password == pass)
                        .FirstOrDefault();

                    if (us == null)
                    {
                        TempData["ccheck"] = -1;
                        RedirectToAction("ChangePass", "User");
                    }
                    else
                    {
                        string newpass = StringUtils.MD5(tNPassWord);
                        us.Password = newpass;
                        using (var ctx = new QuanLyDauGiaEntities())
                        {
                            ctx.Entry(us).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                        }
                        TempData["ccheck"] = 1;
                    }
                    return RedirectToAction("ChangePass", "User");
                }
                catch (Exception)
                {
                    TempData["ccheck"] = -1;
                }
                return RedirectToAction("ChangePass", "User");
            }
        }
        // GET: User/SanPhamDangBan
        [CheckLogin]
        public ActionResult SanPhamDangBan()
        {
            return View();
        }
        [CheckLogin]
        public ActionResult SanPhamDangDauGia()
        {
            return View();
        }
        [CheckLogin]
        public ActionResult ListDauGiaThang()
        {
            return View();
        }
        [CheckLogin]
        public ActionResult Favorite()
        {
            string username = ((User)Session["user"]).UserName;
            using (var ctx = new QuanLyDauGiaEntities())
            {
                ViewBag.Loai = 1;
                List<WatchListVM> list = (from p in ctx.Products
                                          join w in ctx.WatchLists on p.ProID equals w.ProID
                                          where w.UserName == username
                                          select new WatchListVM
                                          {
                                              ProID = p.ProID,
                                              ProName = p.ProName,
                                              UserName = username,
                                              Price = p.Price,
                                              AucPrice = (double)p.AucPrice,
                                              EndTime = p.EndTime,
                                              StartTime = p.StartTime
                                          }).ToList();
                return View();
            }
        }
        // GET: Test
        [CheckLogin]
        public ActionResult MyProducts()
        {
            return View();
        }

        // GET: Test
        [CheckLogin]
        [HttpPost]
        public ActionResult MyProducts(string s)
        {
            return View();
        }

        [CheckLogin]
        public ActionResult AHistoryProduct(int? id)
        {
            int t = Convert.ToInt32(TempData["rcheck"]);
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
            if (id.HasValue == false)
            {
                return RedirectToAction("MyProducts", "User");
            }

            using (var ctx = new QuanLyDauGiaEntities())
            {
                var model = ctx.AuctionHistorys.Where(p => p.ProID == id).OrderBy(p => p.AucPrice).ToList();
                @ViewBag.proidd = id;
                return View();
            }
        }

        [CheckLogin]
        [HttpPost]
        public ActionResult AHistoryProduct(string s)
        {
            return View();
        }

        [CheckLogin]
        public ActionResult SoldProducts()
        {
            return View();
        }
        [CheckLogin]
        public ActionResult WinProducts()
        {
            return View();
        }
        [CheckLogin]
        [HttpPost]
        public ActionResult RemoveUserFromAuc(string proId, string user)
        {
            int t = int.Parse(proId);
            try
            {
                using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
                {
                    User us = dt.Users
                        .Where(p => p.UserName == user)
                        .FirstOrDefault();
                    AuctionHistory au = dt.AuctionHistorys.Where(p => p.ProID == t && p.UserName == user).OrderBy(p => p.AucPrice).FirstOrDefault();
                    var AllAu = dt.AuctionHistorys.Where(p => p.ProID == t).ToList();
                    if (us != null)
                    {
                        int countdel = 0;
                        foreach (var c in AllAu)
                        {
                            if (c.Time >= au.Time)
                            {
                                dt.Entry(c).State = System.Data.Entity.EntityState.Deleted;
                                dt.SaveChanges();
                                countdel++;
                            }
                            else
                            {
                                //khong lam gi ca
                            }
                        }
                        //lấy người đấu giá cuối cùng sau khi chặn người đấu giá trước đó
                        AuctionHistory newau = dt.AuctionHistorys.Where(p => p.ProID == t).OrderByDescending(p => p.AucPrice).FirstOrDefault();
                        //lấy ra sản phẩm sẽ cập nhật lại giá
                        Product lpro = dt.Products.Where(p => p.ProID == t).FirstOrDefault();
                        if (newau != null)
                        {
                            lpro.Owner = newau.UserName;
                            lpro.OwnerPrice = newau.AucPrice;
                            lpro.NumOfAuction -= countdel;
                        }
                        else
                        {
                            lpro.Owner = null;
                            lpro.OwnerPrice = 0;
                        }
                        dt.Entry(lpro).State = System.Data.Entity.EntityState.Modified;
                        dt.SaveChanges();

                        var l = new LimitedList();
                        l.ProID = int.Parse(proId);
                        l.UserName = user;

                        dt.Entry(l).State = System.Data.Entity.EntityState.Added;
                        dt.SaveChanges();
                        TempData["rcheck"] = 1;
                    }
                }
            }
            catch (Exception)
            {
                TempData["rcheck"] = -1;
            }
            return RedirectToAction("AHistoryProduct", "User", new { ID = proId });
        }

        [CheckLogin]
        public ActionResult IsBidding()
        {
            return View();
        }

        [CheckLogin]
        public ActionResult UnexpiredProducts()
        {
            return View();
        }

    }
}