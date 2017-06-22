using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using WebDauGia.Helper;
using WebDauGia.Models;
using WebDauGia.Filters;
using System.Text;
using System.Net.Mail;

namespace WebDauGia.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("Profile", "User");
        }
        // GET: Test
        [CheckLogin]
        public ActionResult Test()
        {
            //string ID = CurrentContext.GetCurUser().UserName;
            //using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            //{
            //    User us = dt.Users
            //        .Where(p => p.UserName == ID.ToString())
            //        .FirstOrDefault();
            //    if (us != null)
            //    {
            //        return View(us);
            //    }
            //}
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
                if (CheckExistEmail.isChecked(model.Email) == 0)
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Email không hợp lệ!!')</script>");
                    View(model);
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
                            u.Reliability = "1/0";
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
                    //xử lý lịch sử
                    //lấy danh các khách hàng tham gia đấu giá sản phẩm có proid như trên.
                    var ListAuhis = dt.AuctionHistorys.Where(auhis => auhis.ProID == t).OrderBy(auhis => auhis.AucPrice).ToList();
                    //Xóa Các Dòng Có tên Người Bị Xóa
                    for (int i = 0; i < ListAuhis.Count; i++)
                    {
                        if (ListAuhis[i].UserName == user)
                        {
                            ListAuhis.RemoveAt(i);
                            i--;
                        }
                    }
                    //sau khi xoa các dòng thì ta có danh sách còn lại
                    //xoa user giống nhau xóa cái ở đằng sau
                    for (int i = 0, j = i + 1; i < ListAuhis.Count - 1; i++, j = i + 1)
                    {

                        if (ListAuhis[i].UserName == ListAuhis[j].UserName)
                        {
                            ListAuhis.Remove(ListAuhis[j]);
                            i--;
                        }
                    }
                    //xóa từ user này 
                    AuctionHistory userchose = null;
                    if (ListAuhis.Count > 0)
                    {
                        userchose = ListAuhis[ListAuhis.Count - 1];
                    }
                    //lay product
                    var pro = dt.Products.Where(p => p.ProID == t).FirstOrDefault();
                    //kiem tra neu user bị kic là owner thì update
                    if (pro.Owner == user)
                    {
                        if (userchose == null)
                        {
                            pro.Owner = null;
                            pro.OwnerPrice = 0;
                            pro.AucPrice = pro.StepPrice;
                        }
                        else
                        {
                            pro.Owner = userchose.UserName;
                            pro.OwnerPrice = userchose.AucPrice;
                            pro.AucPrice = userchose.AucPrice;
                        }
                        dt.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                        dt.SaveChanges();
                    }
                    //xoa lich sử của người bị kíc
                    var list = dt.AuctionHistorys.Where(li => li.UserName == user && li.ProID == t).ToList();
                    dt.AuctionHistorys.RemoveRange(list);
                    dt.SaveChanges();
                    //xóa những lịch sử thừa
                    if (pro.Owner != user && userchose != null)
                    {
                        var list1 = dt.AuctionHistorys.Where(li => li.ProID == t && li.AucID > userchose.AucID).ToList();
                        if (list1 != null)
                        {
                            dt.AuctionHistorys.RemoveRange(list1);
                            dt.SaveChanges();
                        }
                    }
                    //thêm user vào danh sách cấm
                    var l = new LimitedList();
                    l.ProID = int.Parse(proId);
                    l.UserName = user;
                    dt.Entry(l).State = System.Data.Entity.EntityState.Added;
                    dt.SaveChanges();
                    TempData["rcheck"] = 1;
                    //update lại so luot dau
                    var au = dt.AuctionHistorys.Where(a => a.ProID == t).ToList();
                    int sl = 0;
                    if (au.Count > 0)
                    {
                        sl = au.Count;
                    }
                    pro.NumOfAuction = sl;
                    dt.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    dt.SaveChanges();
                    //gửi gmail thông báo
                    StringBuilder Body = new StringBuilder();
                    Body.Append("<h3>Chào: <b>" + user + "<b></h3>");
                    Body.Append("<p>Vào Lúc " + DateTime.Now.ToString() + " Bạn Đã Bị Chủ Sản Phẩm Loại Khỏi Phiên Đấu Giá Sản Phẩm: " + pro.ProName + "</p>");
                    Body.Append("<p style='color:blue'>Chúng Tôi Rất Tiếc Về Điều Này! Còn Rất Nhiều Sản Phẩm Bạn Có Thê Tham Gia Ở Web Của Chúng Tôi!</p>");
                    Body.Append("<table>");
                    Body.Append("<tr><td colspan='2'><h4>Thông tin Sản Phẩm</h4></td></tr>");
                    Body.Append("<tr><td>Tên Sản Phẩm: </td><td>" + pro.ProName + "</td></tr>");
                    Body.Append("<tr><td>Giá Hiện Tại: </td><td>" + string.Format("{0:N0}", pro.AucPrice) + " VNĐ</td></tr>");
                    Body.Append("<tr><td>Thời Gian Kết Thúc Đấu Giá: </td><td>" + pro.EndTime.ToString() + "</td></tr>");
                    Body.Append("</table>");
                    var Use = dt.Users.Where(us => us.UserName == user).FirstOrDefault();
                    MailMessage mail = new MailMessage();
                    mail.To.Add(Use.Email);
                    mail.From = new MailAddress("admiweb2nhom5@gmail.com");
                    mail.Subject = "Thông Báo Bị Loại Khỏi Phiên Đấu Giá Sản Phẩm: " + pro.ProName;
                    mail.Body = Body.ToString();// phần thân của mail ở trên
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    //User us = dt.Users
                    //    .Where(p => p.UserName == user)
                    //    .FirstOrDefault();
                    //AuctionHistory au = dt.AuctionHistorys.Where(p => p.ProID == t && p.UserName == user).OrderBy(p => p.AucPrice).FirstOrDefault();
                    //var AllAu = dt.AuctionHistorys.Where(p=>p.ProID==t).ToList();
                    //if (us != null)
                    //{
                    //    foreach(var c in AllAu)
                    //    {
                    //        if (c.Time >= au.Time)
                    //        {
                    //            dt.Entry(c).State = System.Data.Entity.EntityState.Deleted;
                    //            dt.SaveChanges();
                    //        }
                    //        else
                    //        {
                    //            //khong lam gi ca
                    //        }
                    //    }
                    //    //lấy người đấu giá cuối cùng sau khi chặn người đấu giá trước đó
                    //    AuctionHistory newau = dt.AuctionHistorys.Where(p => p.ProID == t).OrderByDescending(p => p.AucPrice).FirstOrDefault();
                    //    //lấy ra sản phẩm sẽ cập nhật lại giá
                    //    Product lpro = dt.Products.Where(p => p.ProID == t).FirstOrDefault();
                    //    if (newau != null)
                    //    {
                    //        lpro.Owner = newau.UserName;
                    //        lpro.OwnerPrice = newau.AucPrice;
                    //    }
                    //    else
                    //    {
                    //        lpro.Owner = null;
                    //        lpro.OwnerPrice = 0;
                    //    }
                    //    dt.Entry(lpro).State = System.Data.Entity.EntityState.Modified;
                    //    dt.SaveChanges();

                    //    var l = new LimitedList();
                    //    l.ProID = int.Parse(proId);
                    //    l.UserName = user;

                    //    dt.Entry(l).State = System.Data.Entity.EntityState.Added;
                    //    dt.SaveChanges();
                    //    TempData["rcheck"] = 1;
                    //}
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
        [CheckLogin]
        public ActionResult Review(string rec, string proid)
        {
            if (rec == null || rec == "")
            {
                return RedirectToAction("Profile", "User");
            }
            else
            {
                @ViewBag.Receiver = rec;
                @ViewBag.ProId = proid;
            }
            return View();
        }
        [CheckLogin]
        [HttpPost]
        public ActionResult Review(string Sender, string Receiver, string Content,string ProID)
        {
            using (QuanLyDauGiaEntities dt = new QuanLyDauGiaEntities())
            {
                var nx = new Apprais();
                nx.Assessor = Sender;
                nx.BeAsssessed = Receiver;
                nx.Remark = Content;
                nx.TimeAppraise = DateTime.Now;
                // cột id product
                //nx.ProID = ProID;
                //xử lý radio button

                dt.Entry(nx).State = System.Data.Entity.EntityState.Added;
                dt.SaveChanges();
            }

            return View();
        }

        [CheckLogin]
        public ActionResult PurchasedProducts()
        {
            return View();
        }

        [CheckLogin]
        public ActionResult AboutMe()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GuiGM()
        {
            List<Product> index;
            using (QuanLyDauGiaEntities ctx = new QuanLyDauGiaEntities())
            {
                var pro = ctx.Products.Where(p => p.EndTime <= DateTime.Now && (p.Status == false || p.Status == null)).ToList();
                index = pro;
                if (pro.Count > 0)
                {
                    for(int i=0;i<pro.Count;i++)
                    {
                        try
                        {

                            if (pro[i].Owner == null)
                            {
                                string use = pro[i].Salesman;
                                string GmNgBan = (ctx.Users.Where(us => us.UserName == use).FirstOrDefault()).Email;
                                //viet gmail Bán Thất Bại
                                MailMessage mail2 = new MailMessage();
                                mail2.To.Add(GmNgBan);
                                mail2.From = new MailAddress("admiweb2nhom5@gmail.com");
                                mail2.Subject = "Thông Báo Sản Phẩm Của Bạn Không Có Người Đâu Giá " + pro[i].ProName;
                                mail2.Body = Function.GmailBanTB(pro[i], pro[i].Salesman).ToString();
                                mail2.IsBodyHtml = true;
                                SmtpClient smtp2 = new SmtpClient();
                                smtp2.Host = "smtp.gmail.com";
                                smtp2.Port = 587;
                                smtp2.UseDefaultCredentials = true;
                                smtp2.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                                smtp2.EnableSsl = true;
                                smtp2.Send(mail2);
                                pro[i].Status = true;
                                ctx.Entry(pro[i]).State = System.Data.Entity.EntityState.Modified;
                                ctx.SaveChanges();
                            }
                            else if (pro[i].Owner != null)
                            {
                                string useNM = pro[i].Owner;
                                string useNB = pro[i].Salesman;
                                string GmNgMua = (ctx.Users.Where(us => us.UserName == useNM).FirstOrDefault()).Email;
                                string GmNgBan = (ctx.Users.Where(us => us.UserName == useNB).FirstOrDefault()).Email;
                                //gui ngươi ban
                                MailMessage mail2 = new MailMessage();
                                mail2.To.Add(GmNgBan);
                                mail2.From = new MailAddress("admiweb2nhom5@gmail.com");
                                mail2.Subject = "Thông Báo Đã Có Người Mua Thành Công Sản Phẩm Của Bạn " + pro[i].ProName;
                                mail2.Body = Function.GmailBanTC(pro[i], pro[i].Salesman).ToString();
                                mail2.IsBodyHtml = true;
                                SmtpClient smtp2 = new SmtpClient();
                                smtp2.Host = "smtp.gmail.com";
                                smtp2.Port = 587;
                                smtp2.UseDefaultCredentials = true;
                                smtp2.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                                smtp2.EnableSsl = true;
                                smtp2.Send(mail2);
                                //gui nguoi mua
                                MailMessage mail = new MailMessage();
                                mail.To.Add(GmNgMua);
                                mail.From = new MailAddress("admiweb2nhom5@gmail.com");
                                mail.Subject = "Thông Báo Thắng Cuộc Trong Phiên Đấu Giá Sản Phẩm " + pro[i].ProName;
                                mail.Body = Function.GmailTBChienThang(pro[i], pro[i].Owner).ToString();
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.Port = 587;
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = new System.Net.NetworkCredential("admiweb2nhom5@gmail.com", "dakunchan");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                                //
                                pro[i].Status = true;
                                ctx.Entry(pro[i]).State = System.Data.Entity.EntityState.Modified;
                                ctx.SaveChanges();
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}